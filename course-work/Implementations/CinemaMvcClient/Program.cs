namespace CinemaMvcClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Зареждане на ApiSettings от appsettings.json
            builder.Services.Configure<ApiSettings>(
                builder.Configuration.GetSection("ApiSettings"));

            var apiSettings = builder.Configuration
                .GetSection("ApiSettings")
                .Get<ApiSettings>();

            //builder.Services.AddHttpClient<IMovieService, MovieService>(client =>
            //{
            //    client.BaseAddress = new Uri(apiSettings.BaseUrl);
            //});


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
