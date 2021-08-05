using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E90DashboardSimhub {
    //Based on https://github.com/bakercp/PacketSerial/blob/master/src/Encoding/COBS.h
    public class COBS {
        private byte[] buffer = new byte[256];

        public byte[] Decode(byte[] bytes) {
            throw new NotImplementedException();
        }

        public byte[] Encode(byte[] bytes) {
            int encodedMaxSize = GetEncodedBufferSize(bytes.Length);
            int size = bytes.Length;
            int readIndex = 0;
            int writeIndex = 1;
            int codeIndex = 0;
            byte code = 1;

            if(buffer.Length < encodedMaxSize){
                buffer = new byte[encodedMaxSize];
            }

            while (readIndex < size) {
                if(bytes[readIndex] == 0) {
                    buffer[codeIndex] = code;
                    code = 1;
                    codeIndex = writeIndex++;
                    readIndex++;
                } else {
                    buffer[writeIndex++] = bytes[readIndex++];
                    code++;
                    if(code == 0xFF) {
                        buffer[codeIndex] = code;
                        code = 1;
                        codeIndex = writeIndex++;
                    }
                }
            }

            buffer[codeIndex] = code;
            byte[] returnArray = new byte[writeIndex];
            Buffer.BlockCopy(buffer, 0, returnArray, 0, writeIndex);
            return returnArray;
        }

        public static int GetEncodedBufferSize(int unencodedBufferSize) {
            return unencodedBufferSize + unencodedBufferSize / 254 + 1;
        }
    }
}
