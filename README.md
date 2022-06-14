[![Build](https://github.com/versx/Retweety/workflows/.NET%205.0/badge.svg)](https://github.com/versx/Retweety/actions)
[![GitHub Release](https://img.shields.io/github/release/versx/Retweety.svg)](https://github.com/versx/Retweety/releases/)
[![Discord](https://img.shields.io/discord/552003258000998401.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/zZ9h9Xa)  


# Pokemon Go Version Monitor  
Checks and compares latest forced version of Pokemon Go app, when a new version is released a Discord embed is sent via webhooks.  

## Prerequisites  
- [.NET 5 SDK or higher](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)  

## Getting Started  

### _Automated_  
1. Run automated install script (installs .NET 5 SDK and clones repository):  
```
curl https://raw.githubusercontent.com/versx/PoGoVersionMonitor/master/scripts/install.sh > install.sh && chmod +x install.sh && ./install.sh && rm install.sh
```

### _Manually_  
Alternatively, if you already have .NET 5 SDK installed, run the following commands before proceeding below.  

1. `git clone https://github.com/versx/PoGoVersionMonitor && cd PoGoVersionMonitor`  
2. `dotnet build`  
3. `cp config.example.json bin/config.json`  

---
Once the project is cloned and .NET 5 SDK is installed continue on:  
1. Fill out `bin/config.json` `webhooks` list with Discord webhooks to receive version changed message.  
2. Input the bot properties under the `bot` config section to set the desired name and optional icon url.  
3. Build the executable file `dotnet build`.  
4. Start PoGoVersionMonitor from the `bin` folder: `dotnet PoGoVersionMonitor.dll`.  


## Configuration  
```json
{
    // Bot display settings for embed post
    "bot": {
        // Bot name for Discord embed message
        "name": "PoGo Version Monitor",
        // Bot icon url for Discord embed message (optional)
        "iconUrl": "https://w7.pngwing.com/pngs/652/369/png-transparent-pokemon-go-computer-icons-raid-niantic-pokemongo-video-game-boss-pokemon.png"
    },
    // Webhooks that will receive the version changed message.
    "webhooks": []
}
```