using SharpHash.Base;
using SharpHash.Interfaces;
using SharpHash.Utils;
using System;

namespace SharpHash.Crypto
{
    public abstract class MDBase : BlockHash, ICryptoNotBuildIn
    {
        protected static UInt32 C1 = 0x50A28BE6;
        protected static UInt32 C2 = 0x5A827999;
        protected static UInt32 C3 = 0x5C4DD124;
        protected static UInt32 C4 = 0x6ED9EBA1;
        protected static UInt32 C5 = 0x6D703EF3;
        protected static UInt32 C6 = 0x8F1BBCDC;
        protected static UInt32 C7 = 0x7A6D76E9;
        protected static UInt32 C8 = 0xA953FD4E;

        protected UInt32[] state = null;

        public MDBase(Int32 a_state_length, Int32 a_hash_size)
            : base(a_hash_size, 64)
        {
            Array.Resize(ref state, a_state_length);
        } // end constructor

        override protected unsafe byte[] GetResult()
        {
            byte[] result = new byte[state.Length * sizeof(UInt32)];

            fixed (UInt32* statePtr = state)
            {
                fixed (byte* resultPtr = result)
                {
                    Converters.le32_copy((IntPtr)statePtr, 0, (IntPtr)resultPtr, 0, 
                        state.Length * sizeof(UInt32));
                }
            }

            return result;
        } // end function GetResult

        override public void Initialize()
        {
            state[0] = 0x67452301;
            state[1] = 0xEFCDAB89;
            state[2] = 0x98BADCFE;
            state[3] = 0x10325476;

            base.Initialize();
        } // end function Initialize

        override protected void Finish()
        {
            UInt64 bits;
            Int32 padindex;

            bits = processed_bytes * 8;
            if (buffer.Position < 56)
                padindex = 56 - buffer.Position;
            else
                padindex = 120 - buffer.Position;

            byte[] pad = new byte[padindex + 8];

            pad[0] = 0x80;

            bits = Converters.le2me_64(bits);

            Converters.ReadUInt64AsBytesLE(bits, ref pad, padindex);

            padindex = padindex + 8;

            TransformBytes(pad, 0, padindex);
        } // end function Finish

    } // end class MDBase

}