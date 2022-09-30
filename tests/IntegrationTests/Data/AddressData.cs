
using Bridge.Application.Places.Dtos;

namespace Bridge.IntegrationTests.Data
{
    public class AddressData
    {
        public static AddressDto Seoul1 => new() { BaseAddress = "서울시 강남구 논현로 710" };
        public static AddressDto Seoul2 => new() { BaseAddress = "서울특별시 중구 남대문로5가 395" };
        public static AddressDto Seoul3 => new() { BaseAddress = "서울특별시 성북구 하월곡동 75-32" };
        public static AddressDto Seoul4 => new() { BaseAddress = "서울특별시 양천구 신정동 1198-23" };

        public static AddressDto Daegu1 => new() { BaseAddress = "대구시 수성구 청수로 25길 118-10" };
        public static AddressDto Daegu2 => new() { BaseAddress = "수성구 유니버시아드로42길 127" };
        public static AddressDto Daegu3 => new() { BaseAddress = "대구광역시 달서구 월암동 922-10" };
        public static AddressDto Daegu4 => new() { BaseAddress = "대구광역시 수성구 지산동 1282-8" };
        public static AddressDto Daegu5 => new() { BaseAddress = "대구시 북구 동천로 5-3" };

        public static AddressDto Busan => new(){ BaseAddress = "연제구 중앙대로 1001" };

    }
}
