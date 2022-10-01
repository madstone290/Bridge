using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bridge.Infrastructure.Extensions
{
    public static  class OptionsExtensions
    {
        /// <summary>
        /// 옵션을 서비스 컬렉션에 등록한다
        /// </summary>
        /// <typeparam name="TOptions">옵션 타입</typeparam>
        /// <param name="services">서비스 컬렉션</param>
        /// <param name="configSection">옵션에 바인딩되는 구성 섹션</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static OptionsBuilder<TOptions> AddOptionsEx<TOptions>(this IServiceCollection services, IConfigurationSection configSection)
            where TOptions : class
        {
            if (!configSection.Exists())
                throw new Exception($"Key: {configSection.Key}, Path: {configSection.Path} 설정이 존재하지 않습니다");

            return services.AddOptions<TOptions>()
                .Bind(configSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

     
    }
}
