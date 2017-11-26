using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Thanking.Utilities
{
	#region Enums
	[Flags]
	public enum FileSystemFeature : uint
	{
		/// <summary>
		/// The file system preserves the case of file names when it places a name on disk.
		/// </summary>
		CasePreservedNames = 2,

		/// <summary>
		/// The file system supports case-sensitive file names.
		/// </summary>
		CaseSensitiveSearch = 1,

		/// <summary>
		/// The specified volume is a direct access (DAX) volume. This flag was introduced in Windows 10, version 1607.
		/// </summary>
		DaxVolume = 0x20000000,

		/// <summary>
		/// The file system supports file-based compression.
		/// </summary>
		FileCompression = 0x10,

		/// <summary>
		/// The file system supports named streams.
		/// </summary>
		NamedStreams = 0x40000,

		/// <summary>
		/// The file system preserves and enforces access control lists (ACL).
		/// </summary>
		PersistentACLS = 8,

		/// <summary>
		/// The specified volume is read-only.
		/// </summary>
		ReadOnlyVolume = 0x80000,

		/// <summary>
		/// The volume supports a single sequential write.
		/// </summary>
		SequentialWriteOnce = 0x100000,

		/// <summary>
		/// The file system supports the Encrypted File System (EFS).
		/// </summary>
		SupportsEncryption = 0x20000,

		/// <summary>
		/// The specified volume supports extended attributes. An extended attribute is a piece of
		/// application-specific metadata that an application can associate with a file and is not part
		/// of the file's data.
		/// </summary>
		SupportsExtendedAttributes = 0x00800000,

		/// <summary>
		/// The specified volume supports hard links. For more information, see Hard Links and Junctions.
		/// </summary>
		SupportsHardLinks = 0x00400000,

		/// <summary>
		/// The file system supports object identifiers.
		/// </summary>
		SupportsObjectIDs = 0x10000,

		/// <summary>
		/// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
		/// </summary>
		SupportsOpenByFileId = 0x01000000,

		/// <summary>
		/// The file system supports re-parse points.
		/// </summary>
		SupportsReparsePoints = 0x80,

		/// <summary>
		/// The file system supports sparse files.
		/// </summary>
		SupportsSparseFiles = 0x40,

		/// <summary>
		/// The volume supports transactions.
		/// </summary>
		SupportsTransactions = 0x200000,

		/// <summary>
		/// The specified volume supports update sequence number (USN) journals. For more information,
		/// see Change Journal Records.
		/// </summary>
		SupportsUsnJournal = 0x02000000,

		/// <summary>
		/// The file system supports Unicode in file names as they appear on disk.
		/// </summary>
		UnicodeOnDisk = 4,

		/// <summary>
		/// The specified volume is a compressed volume, for example, a DoubleSpace volume.
		/// </summary>
		VolumeIsCompressed = 0x8000,

		/// <summary>
		/// The file system supports disk quotas.
		/// </summary>
		VolumeQuotas = 0x20
	}

	[Flags]
	public enum AllocationType : uint
	{
		COMMIT = 0x1000,
		RESERVE = 0x2000,
		RESET = 0x80000,
		LARGE_PAGES = 0x20000000,
		PHYSICAL = 0x400000,
		TOP_DOWN = 0x100000,
		WRITE_WATCH = 0x200000
	}

	[Flags]
	public enum MemoryProtection : uint
	{
		EXECUTE = 0x10,
		EXECUTE_READ = 0x20,
		EXECUTE_READWRITE = 0x40,
		EXECUTE_WRITECOPY = 0x80,
		NOACCESS = 0x01,
		READONLY = 0x02,
		READWRITE = 0x04,
		WRITECOPY = 0x08,
		GUARD_Modifierflag = 0x100,
		NOCACHE_Modifierflag = 0x200,
		WRITECOMBINE_Modifierflag = 0x400
	}
	#endregion

	public static class HWIDUtilities
	{
		#region Imports
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool GetVolumeInformation(
			string rootPathName,
			StringBuilder volumeNameBuffer,
			int volumeNameSize,
			out uint volumeSerialNumber,
			out uint maximumComponentLength,
			out FileSystemFeature fileSystemFlags,
			StringBuilder fileSystemNameBuffer,
			int nFileSystemNameSize);

		[DllImport("kernel32.dll")]
		static extern unsafe bool GetComputerNameA(StringBuilder lpBuffer, ref uint lpnSize);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void CpuIDDelegate(int level, byte[] buffer);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType,
			MemoryProtection flProtect);

		[DllImport("kernel32")]
		private static extern bool VirtualFree(IntPtr lpAddress, UInt32 dwSize, UInt32 dwFreeType);
		#endregion

		public static string GetHWID()
		{
			GetVolumeInformation("C:\\", null, 0, out uint sn, out uint i, out FileSystemFeature feat, null, 0);

			uint size = 32;
			StringBuilder builder = new StringBuilder(32);

			GetComputerNameA(builder, ref size);

			string name = "";
			foreach (char c in builder.ToString())
				name += ((byte)c).ToString("x2");

			string cpuinfo = "";
			byte[] id = Invoke(0);

			for (int j = 0; j < 3; j++)
				cpuinfo += BitConverter.ToInt32(id.Skip(j * 4).Take(4).ToArray(), 0);

			string s = $"{cpuinfo}-{sn}-{name}";
			s = HashFNV1a(s.Select(c => (byte)c).ToArray()).ToString();
			s = Convert.ToBase64String(s.Select(c => (byte)c).ToArray());

			return s;
		}
		
		public static ulong HashFNV1a(byte[] bytes)
		{
			const ulong fnv64Offset = 14695981039346656037;
			const ulong fnv64Prime = 0x100000001b3;
			ulong hash = fnv64Offset;

			for (var i = 0; i < bytes.Length; i++)
			{
				hash = hash ^ bytes[i];
				hash *= fnv64Prime;
			}

			return hash;
		}

		public static byte[] Invoke(int level)
		{
			IntPtr codePointer = IntPtr.Zero;
			try
			{
				// compile
				byte[] codeBytes;
				if (IntPtr.Size == 4)
				{
					codeBytes = x86CodeBytes;
				}
				else
				{
					codeBytes = x64CodeBytes;
				}

				codePointer = VirtualAlloc(
					IntPtr.Zero,
					new UIntPtr((uint)codeBytes.Length),
					AllocationType.COMMIT | AllocationType.RESERVE,
					MemoryProtection.EXECUTE_READWRITE
				);

				Marshal.Copy(codeBytes, 0, codePointer, codeBytes.Length);

				CpuIDDelegate cpuIdDelg = (CpuIDDelegate)Marshal.GetDelegateForFunctionPointer(codePointer, typeof(CpuIDDelegate));

				// invoke
				GCHandle handle = default(GCHandle);
				var buffer = new byte[16];

				try
				{
					handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
					cpuIdDelg(level, buffer);
				}
				finally
				{
					if (handle != default(GCHandle))
					{
						handle.Free();
					}
				}

				return buffer;
			}
			finally
			{
				if (codePointer != IntPtr.Zero)
				{
					VirtualFree(codePointer, 0, 0x8000);
					codePointer = IntPtr.Zero;
				}
			}
		}

		// Basic ASM strategy --
		// void x86CpuId(int level, byte* buffer) 
		// {
		//    eax = level
		//    cpuid
		//    buffer[0] = eax
		//    buffer[4] = ebx
		//    buffer[8] = ecx
		//    buffer[12] = edx
		// }

		private readonly static byte[] x86CodeBytes = {
		0x55,                   // push        ebp  
        0x8B, 0xEC,             // mov         ebp,esp
        0x53,                   // push        ebx  
        0x57,                   // push        edi

        0x8B, 0x45, 0x08,       // mov         eax, dword ptr [ebp+8] (move level into eax)
        0x0F, 0xA2,              // cpuid

        0x8B, 0x7D, 0x0C,       // mov         edi, dword ptr [ebp+12] (move address of buffer into edi)
        0x89, 0x07,             // mov         dword ptr [edi+0], eax  (write eax, ... to buffer)
        0x89, 0x5F, 0x04,       // mov         dword ptr [edi+4], ebx 
        0x89, 0x4F, 0x08,       // mov         dword ptr [edi+8], ecx 
        0x89, 0x57, 0x0C,       // mov         dword ptr [edi+12],edx 

        0x5F,                   // pop         edi  
        0x5B,                   // pop         ebx  
        0x8B, 0xE5,             // mov         esp,ebp  
        0x5D,                   // pop         ebp 
        0xc3                    // ret
    };

		private readonly static byte[] x64CodeBytes = {
		0x53,                       // push rbx    this gets clobbered by cpuid

        // rcx is level
        // rdx is buffer.
        // Need to save buffer elsewhere, cpuid overwrites rdx
        // Put buffer in r8, use r8 to reference buffer later.

        // Save rdx (buffer addy) to r8
        0x49, 0x89, 0xd0,           // mov r8,  rdx

        // Move ecx (level) to eax to call cpuid, call cpuid
        0x89, 0xc8,                 // mov eax, ecx
        0x0F, 0xA2,                 // cpuid

        // Write eax et al to buffer
        0x41, 0x89, 0x40, 0x00,     // mov    dword ptr [r8+0],  eax
        0x41, 0x89, 0x58, 0x04,     // mov    dword ptr [r8+4],  ebx
        0x41, 0x89, 0x48, 0x08,     // mov    dword ptr [r8+8],  ecx
        0x41, 0x89, 0x50, 0x0c,     // mov    dword ptr [r8+12], edx

        0x5b,                       // pop rbx
        0xc3                        // ret
    };
	}
}
