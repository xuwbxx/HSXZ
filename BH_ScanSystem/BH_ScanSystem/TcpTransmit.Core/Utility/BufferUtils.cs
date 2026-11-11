using System;
using System.Net.Sockets;

namespace TcpTransmit.Core.Utility {
    internal class BufferUtils {
        public static readonly byte[] Delimiter =
            {
                //0xAA,
                //0xFF,
                //0xAA
            };

        public static readonly int DelimiterLength;

        static BufferUtils() {
            DelimiterLength = Delimiter.Length;
        }

        #region GetPacketBuffer

        public static void GetPacketBuffer(byte[] buffer, int messageBufferSize,
                                           SocketAsyncEventArgs socketAsyncEventArgs) {
            int bufferSize = buffer.Length + DelimiterLength;
            if (bufferSize > messageBufferSize) {
                var result = new byte[bufferSize];
                socketAsyncEventArgs.SetBuffer(result, 0, bufferSize);
            }
            else {
                socketAsyncEventArgs.SetBuffer(0, bufferSize);
            }
            Buffer.BlockCopy(buffer, 0, socketAsyncEventArgs.Buffer, 0, buffer.Length);
            Buffer.BlockCopy(Delimiter, 0, socketAsyncEventArgs.Buffer, buffer.Length, DelimiterLength);
        }

        #endregion

        #region GetRawBuffer

        public static byte[] GetRawBuffer(BaseSocketConnection connection, byte[] buffer, int readBytes) {
            var result = new byte[readBytes];
            Buffer.BlockCopy(buffer, 0, result, 0, readBytes);
            return result;
        }

        #endregion

        #region GetRawBufferWithTail

        public static byte[] GetRawBufferWithTail(BaseSocketConnection connection, SocketAsyncEventArgs e, int position,
                                                  int begin) {
            //----- Get Raw Buffer with Tail!


            var result = new byte[position - begin  - DelimiterLength];


            Buffer.BlockCopy(e.Buffer, begin, result, 0, result.Length);


            return result;
        }

        #endregion
    }
}