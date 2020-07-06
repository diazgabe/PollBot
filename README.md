# PollBot
 
## Installation
To host your own instance of the PollBot, clone this repository using [Visual Studio 2019](https://www.visualstudio.com/downloads/) or [download the release package](https://github.com/diazgabe/PollBot/releases) and update the  **config.json** file.

## Configuration
```
  prefix             The prefix of commands that this bot will respond to. Default: "!"
  token              Your Discord bot token
```

## Commands
```
  !help              Displays the help message
  !deleteCommands    Configures the bot to delete commands after executing them
  !keepCommands      Configures the bot to keep commands after executing them
  !poll              Creates a reaction poll. Syntax is as follows: !poll [option1(reaction1) ... optionN(reactionN)]  
                          Example: !poll a(😄), b(😜), c(😍)
```

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
