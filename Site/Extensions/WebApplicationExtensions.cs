namespace SystemAggregator.Site
{
    public static class WebApplicationExtensions
    {
        public static void UseApiPipeline(this WebApplication app)
        {
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapControllers();
            });
        }
    }
}
