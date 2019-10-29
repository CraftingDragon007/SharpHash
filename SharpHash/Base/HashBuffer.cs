﻿///////////////////////////////////////////////////////////////////////
/// SharpHash Library
/// Copyright(c) 2019  Mbadiwe Nnaemeka Ronald
/// Github Repository <https://github.com/ron4fun/SharpHash>
///
/// The contents of this file are subject to the
/// Mozilla Public License Version 2.0 (the "License");
/// you may not use this file except in
/// compliance with the License. You may obtain a copy of the License
/// at https://www.mozilla.org/en-US/MPL/2.0/
///
/// Software distributed under the License is distributed on an "AS IS"
/// basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
/// the License for the specific language governing rights and
/// limitations under the License.
///
/// Acknowledgements:
///
/// Thanks to Ugochukwu Mmaduekwe (https://github.com/Xor-el) for his creative
/// development of this library in Pascal/Delphi (https://github.com/Xor-el/HashLib4Pascal).
///
/// Also, I will like to thank Udezue Chukwunwike (https://github.com/IzarchTech) for
/// his contributions to the growth and development of this library.
///
////////////////////////////////////////////////////////////////////////

using System;

namespace SharpHash.Base
{
    internal sealed class HashBuffer
    {
        private byte[] data = null;
        private Int32 pos = 0;

        public HashBuffer(Int32 a_length)
        {
            data = new byte[a_length];
            Initialize();
        } // end constructor

        public HashBuffer Clone()
        {
            HashBuffer result = new HashBuffer(0);

            result.pos = pos;

            result.data = new byte[data.Length];
            Utils.Utils.memcopy(ref result.data, data, data.Length);

            return result;
        }

        public unsafe bool Feed(IntPtr a_data, Int32 a_length_a_data, Int32 a_length)
        {
            Int32 Length;

            if (a_length_a_data == 0)
            {
                return false;
            } // end if

            if (a_length == 0)
            {
                return false;
            } // end if

            Length = data.Length - pos;
            if (Length > a_length)
            {
                Length = a_length;
            } // end if

            fixed (byte* bDest = &data[0])
            {
                Utils.Utils.memmove((IntPtr)bDest, a_data, Length * sizeof(byte));
            }

            pos = pos + Length;

            return IsFull;
        } // end function Feed

        public unsafe bool Feed(IntPtr a_data, Int32 a_length_a_data,
            ref Int32 a_start_index, ref Int32 a_length, ref UInt64 a_processed_bytes)
        {
            Int32 Length;

            if (a_length_a_data == 0)
            {
                return false;
            } // end if

            if (a_length == 0)
            {
                return false;
            } // end if

            Length = data.Length - pos;
            if (Length > a_length)
            {
                Length = a_length;
            } // end if

            fixed (byte* bDest = &data[pos])
            {
                Utils.Utils.memmove((IntPtr)bDest, (IntPtr)((byte*)a_data + a_start_index), Length * sizeof(byte));
            }

            pos = pos + Length;
            a_start_index = a_start_index + Length;
            a_length = a_length - Length;
            a_processed_bytes = a_processed_bytes + (UInt64)(Length);

            return IsFull;
        } // end function Feed

        public byte[] GetBytes()
        {
            pos = 0;

            byte[] result = new byte[data.Length];
            Utils.Utils.memcopy(ref result, data, data.Length);

            return result;
        } // end function GetBytes

        public unsafe byte[] GetBytesZeroPadded()
        {
            Utils.Utils.memset(ref data, 0, pos);

            pos = 0;

            byte[] result = new byte[data.Length];
            Utils.Utils.memcopy(ref result, data, data.Length);

            return result;
        } // end function GetBytesZeroPadded

        public bool IsEmpty => pos == 0;

        public bool IsFull => pos == data.Length;

        public Int32 Length => data.Length; // end property Length

        public Int32 Position => pos;

        public void Initialize()
        {
            pos = 0;

            Utils.Utils.memset(ref data, 0);
        } // end function Initialize

        public override string ToString()
        {
            return $"HashBuffer, Length: {Length}, Pos: {Position}, IsEmpty: {IsEmpty}";
        } // end function ToString
    }
}