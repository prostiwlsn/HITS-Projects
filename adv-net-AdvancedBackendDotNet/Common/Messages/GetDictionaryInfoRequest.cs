using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class GetDictionaryInfoRequest
    {
        public DictionaryInfoType Type {  get; set; }
        public string Id { get; set; } //переводить в guid или строку
    }
}
