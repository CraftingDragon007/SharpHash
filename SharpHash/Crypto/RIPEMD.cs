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

using SharpHash.Interfaces;
using SharpHash.Utils;
using System;

namespace SharpHash.Crypto
{
    internal sealed class RIPEMD : MDBase, ITransformBlock
    {
        private UInt32[] data = null;

        public RIPEMD()
            : base(4, 16)
        {
            data = new UInt32[16];
        } // end constructor

        public override IHash Clone()
        {
            RIPEMD HashInstance = new RIPEMD();
            HashInstance.buffer = buffer.Clone();
            HashInstance.processed_bytes = processed_bytes;

            HashInstance.state = state.DeepCopy();

            HashInstance.BufferSize = BufferSize;

            return HashInstance;
        } // end function Clone

        private static UInt32 P1(UInt32 a, UInt32 b, UInt32 c)
        {
            return (a & b) | (~a & c);
        } // end function P1

        private static UInt32 P2(UInt32 a, UInt32 b, UInt32 c)
        {
            return (a & b) | (a & c) | (b & c);
        } // end function P2

        private static UInt32 P3(UInt32 a, UInt32 b, UInt32 c)
        {
            return a ^ b ^ c;
        } // end function P3

        protected override unsafe void TransformBlock(IntPtr a_data,
                Int32 a_data_length, Int32 a_index)
        {
            UInt32 a, b, c, d, aa, bb, cc, dd;

            fixed (UInt32* dPtr = data)
            {
                Converters.le32_copy(a_data, a_index, (IntPtr)dPtr, 0, 64);
            }

            a = state[0];
            b = state[1];
            c = state[2];
            d = state[3];
            aa = a;
            bb = b;
            cc = c;
            dd = d;

            a = Bits.RotateLeft32(P1(b, c, d) + a + data[0], 11);
            d = Bits.RotateLeft32(P1(a, b, c) + d + data[1], 14);
            c = Bits.RotateLeft32(P1(d, a, b) + c + data[2], 15);
            b = Bits.RotateLeft32(P1(c, d, a) + b + data[3], 12);
            a = Bits.RotateLeft32(P1(b, c, d) + a + data[4], 5);
            d = Bits.RotateLeft32(P1(a, b, c) + d + data[5], 8);
            c = Bits.RotateLeft32(P1(d, a, b) + c + data[6], 7);
            b = Bits.RotateLeft32(P1(c, d, a) + b + data[7], 9);
            a = Bits.RotateLeft32(P1(b, c, d) + a + data[8], 11);
            d = Bits.RotateLeft32(P1(a, b, c) + d + data[9], 13);
            c = Bits.RotateLeft32(P1(d, a, b) + c + data[10], 14);
            b = Bits.RotateLeft32(P1(c, d, a) + b + data[11], 15);
            a = Bits.RotateLeft32(P1(b, c, d) + a + data[12], 6);
            d = Bits.RotateLeft32(P1(a, b, c) + d + data[13], 7);
            c = Bits.RotateLeft32(P1(d, a, b) + c + data[14], 9);
            b = Bits.RotateLeft32(P1(c, d, a) + b + data[15], 8);

            a = Bits.RotateLeft32(P2(b, c, d) + a + data[7] + C2, 7);
            d = Bits.RotateLeft32(P2(a, b, c) + d + data[4] + C2, 6);
            c = Bits.RotateLeft32(P2(d, a, b) + c + data[13] + C2, 8);
            b = Bits.RotateLeft32(P2(c, d, a) + b + data[1] + C2, 13);
            a = Bits.RotateLeft32(P2(b, c, d) + a + data[10] + C2, 11);
            d = Bits.RotateLeft32(P2(a, b, c) + d + data[6] + C2, 9);
            c = Bits.RotateLeft32(P2(d, a, b) + c + data[15] + C2, 7);
            b = Bits.RotateLeft32(P2(c, d, a) + b + data[3] + C2, 15);
            a = Bits.RotateLeft32(P2(b, c, d) + a + data[12] + C2, 7);
            d = Bits.RotateLeft32(P2(a, b, c) + d + data[0] + C2, 12);
            c = Bits.RotateLeft32(P2(d, a, b) + c + data[9] + C2, 15);
            b = Bits.RotateLeft32(P2(c, d, a) + b + data[5] + C2, 9);
            a = Bits.RotateLeft32(P2(b, c, d) + a + data[14] + C2, 7);
            d = Bits.RotateLeft32(P2(a, b, c) + d + data[2] + C2, 11);
            c = Bits.RotateLeft32(P2(d, a, b) + c + data[11] + C2, 13);
            b = Bits.RotateLeft32(P2(c, d, a) + b + data[8] + C2, 12);

            a = Bits.RotateLeft32(P3(b, c, d) + a + data[3] + C4, 11);
            d = Bits.RotateLeft32(P3(a, b, c) + d + data[10] + C4, 13);
            c = Bits.RotateLeft32(P3(d, a, b) + c + data[2] + C4, 14);
            b = Bits.RotateLeft32(P3(c, d, a) + b + data[4] + C4, 7);
            a = Bits.RotateLeft32(P3(b, c, d) + a + data[9] + C4, 14);
            d = Bits.RotateLeft32(P3(a, b, c) + d + data[15] + C4, 9);
            c = Bits.RotateLeft32(P3(d, a, b) + c + data[8] + C4, 13);
            b = Bits.RotateLeft32(P3(c, d, a) + b + data[1] + C4, 15);
            a = Bits.RotateLeft32(P3(b, c, d) + a + data[14] + C4, 6);
            d = Bits.RotateLeft32(P3(a, b, c) + d + data[7] + C4, 8);
            c = Bits.RotateLeft32(P3(d, a, b) + c + data[0] + C4, 13);
            b = Bits.RotateLeft32(P3(c, d, a) + b + data[6] + C4, 6);
            a = Bits.RotateLeft32(P3(b, c, d) + a + data[11] + C4, 12);
            d = Bits.RotateLeft32(P3(a, b, c) + d + data[13] + C4, 5);
            c = Bits.RotateLeft32(P3(d, a, b) + c + data[5] + C4, 7);
            b = Bits.RotateLeft32(P3(c, d, a) + b + data[12] + C4, 5);

            aa = Bits.RotateLeft32(P1(bb, cc, dd) + aa + data[0] + C1, 11);
            dd = Bits.RotateLeft32(P1(aa, bb, cc) + dd + data[1] + C1, 14);
            cc = Bits.RotateLeft32(P1(dd, aa, bb) + cc + data[2] + C1, 15);
            bb = Bits.RotateLeft32(P1(cc, dd, aa) + bb + data[3] + C1, 12);
            aa = Bits.RotateLeft32(P1(bb, cc, dd) + aa + data[4] + C1, 5);
            dd = Bits.RotateLeft32(P1(aa, bb, cc) + dd + data[5] + C1, 8);
            cc = Bits.RotateLeft32(P1(dd, aa, bb) + cc + data[6] + C1, 7);
            bb = Bits.RotateLeft32(P1(cc, dd, aa) + bb + data[7] + C1, 9);
            aa = Bits.RotateLeft32(P1(bb, cc, dd) + aa + data[8] + C1, 11);
            dd = Bits.RotateLeft32(P1(aa, bb, cc) + dd + data[9] + C1, 13);
            cc = Bits.RotateLeft32(P1(dd, aa, bb) + cc + data[10] + C1, 14);
            bb = Bits.RotateLeft32(P1(cc, dd, aa) + bb + data[11] + C1, 15);
            aa = Bits.RotateLeft32(P1(bb, cc, dd) + aa + data[12] + C1, 6);
            dd = Bits.RotateLeft32(P1(aa, bb, cc) + dd + data[13] + C1, 7);
            cc = Bits.RotateLeft32(P1(dd, aa, bb) + cc + data[14] + C1, 9);
            bb = Bits.RotateLeft32(P1(cc, dd, aa) + bb + data[15] + C1, 8);

            aa = Bits.RotateLeft32(P2(bb, cc, dd) + aa + data[7], 7);
            dd = Bits.RotateLeft32(P2(aa, bb, cc) + dd + data[4], 6);
            cc = Bits.RotateLeft32(P2(dd, aa, bb) + cc + data[13], 8);
            bb = Bits.RotateLeft32(P2(cc, dd, aa) + bb + data[1], 13);
            aa = Bits.RotateLeft32(P2(bb, cc, dd) + aa + data[10], 11);
            dd = Bits.RotateLeft32(P2(aa, bb, cc) + dd + data[6], 9);
            cc = Bits.RotateLeft32(P2(dd, aa, bb) + cc + data[15], 7);
            bb = Bits.RotateLeft32(P2(cc, dd, aa) + bb + data[3], 15);
            aa = Bits.RotateLeft32(P2(bb, cc, dd) + aa + data[12], 7);
            dd = Bits.RotateLeft32(P2(aa, bb, cc) + dd + data[0], 12);
            cc = Bits.RotateLeft32(P2(dd, aa, bb) + cc + data[9], 15);
            bb = Bits.RotateLeft32(P2(cc, dd, aa) + bb + data[5], 9);
            aa = Bits.RotateLeft32(P2(bb, cc, dd) + aa + data[14], 7);
            dd = Bits.RotateLeft32(P2(aa, bb, cc) + dd + data[2], 11);
            cc = Bits.RotateLeft32(P2(dd, aa, bb) + cc + data[11], 13);
            bb = Bits.RotateLeft32(P2(cc, dd, aa) + bb + data[8], 12);

            aa = Bits.RotateLeft32(P3(bb, cc, dd) + aa + data[3] + C3, 11);
            dd = Bits.RotateLeft32(P3(aa, bb, cc) + dd + data[10] + C3, 13);
            cc = Bits.RotateLeft32(P3(dd, aa, bb) + cc + data[2] + C3, 14);
            bb = Bits.RotateLeft32(P3(cc, dd, aa) + bb + data[4] + C3, 7);
            aa = Bits.RotateLeft32(P3(bb, cc, dd) + aa + data[9] + C3, 14);
            dd = Bits.RotateLeft32(P3(aa, bb, cc) + dd + data[15] + C3, 9);
            cc = Bits.RotateLeft32(P3(dd, aa, bb) + cc + data[8] + C3, 13);
            bb = Bits.RotateLeft32(P3(cc, dd, aa) + bb + data[1] + C3, 15);
            aa = Bits.RotateLeft32(P3(bb, cc, dd) + aa + data[14] + C3, 6);
            dd = Bits.RotateLeft32(P3(aa, bb, cc) + dd + data[7] + C3, 8);
            cc = Bits.RotateLeft32(P3(dd, aa, bb) + cc + data[0] + C3, 13);
            bb = Bits.RotateLeft32(P3(cc, dd, aa) + bb + data[6] + C3, 6);
            aa = Bits.RotateLeft32(P3(bb, cc, dd) + aa + data[11] + C3, 12);
            dd = Bits.RotateLeft32(P3(aa, bb, cc) + dd + data[13] + C3, 5);
            cc = Bits.RotateLeft32(P3(dd, aa, bb) + cc + data[5] + C3, 7);
            bb = Bits.RotateLeft32(P3(cc, dd, aa) + bb + data[12] + C3, 5);

            cc = cc + state[0] + b;
            state[0] = state[1] + c + dd;
            state[1] = state[2] + d + aa;
            state[2] = state[3] + a + bb;
            state[3] = cc;

            Utils.Utils.Memset(ref data, 0);
        } // end function TransformBlock
    } // end class RIPEMD
}