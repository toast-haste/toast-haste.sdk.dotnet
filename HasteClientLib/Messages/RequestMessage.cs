﻿/*
* Copyright 2016 NHN Entertainment Corp.
*
* NHN Entertainment Corp. licenses this file to you under the Apache License,
* version 2.0 (the "License"); you may not use this file except in compliance
* with the License. You may obtain a copy of the License at:
*
*   http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using Haste.Data;

namespace Haste.Messages
{
    public sealed class RequestMessage : MessageBase
    {
        private readonly byte[] _bytes;

        public RequestMessage(short code, DataObject parameters)
        {
            this._code = code;
            this._data = parameters;
        }

        public RequestMessage(short code, DataObject parameters, INetworkPeer peer, bool encrypt)
        {
            var buffer = ByteBufferFactory.NewBuffer();

            // Write a message header (2 bytes).
            buffer.WriteByte(DataSerializer.Version);
            buffer.WriteByte((byte)MessageType.RequestMessage);
            
            buffer.WriteShort(code);
            buffer.WriteDataObject(parameters);

            if (encrypt)
            {
                byte[] bytes = buffer.ToArray();
                _bytes = peer.Cipher.Encrypt(bytes);
            }
            else
            {
                _bytes = buffer.ToArray();
            }
        }

        public byte[] GetBytes()
        {
            return _bytes;
        }

        public override string ToString()
        {
            return string.Format("RequestMessage " + _code + " Parameters: " + _data);
        }
    }
}