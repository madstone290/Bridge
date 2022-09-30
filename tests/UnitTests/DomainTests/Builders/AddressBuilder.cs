using Bridge.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class AddressBuilder
    {
        public Address SeoulAddress1 { get; } = Address.Create(
           "서울특별시 동대문구 경동시장로 2",
           "서울특별시 동대문구 제기동 655-1",
           "",
           "서울특별시",
           "동대문구",
           "제기동",
           "경동시장로",
           "02572");



        public Address DaeguAddress1 { get; } = Address.Create(
            "대구광역시 수성구 청수로25길 118-10",
            "대구광역시 수성구 황금동 880-7",
            "",
            "대구광역시",
            "수성구",
            "황금동",
            "청수로25길",
            "42148");


    }
}

