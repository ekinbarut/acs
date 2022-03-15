To set up mongo and consul, run the following command first 

```docker-compose up```

Then either run the application on a docker container, or your IDE.

The configuration data will be seeded in consul on startup 

```c#
private void SeedConsulData(IApplicationBuilder app)
{
    var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
    var data = new Dictionary<string, double>()
        {
            {"PhotoBook", 19},
            {"Calendar", 10},
            {"Canvas", 16},
            {"Cards", 4.7},
            {"Mug", 94}
        };
    var config = JsonConvert.SerializeObject(data);
    var bytes = Encoding.UTF8.GetBytes(config);
    consulClient.KV.Put(new KVPair("ProductWidth") {Value = bytes}).Wait();
}
```

You can reach the consul UI on http://localhost:8500/ui/ and the swagger ui on http://localhost:5001/swagger/