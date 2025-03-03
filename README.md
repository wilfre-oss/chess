# My Chess Bot  

This is a chess bot built using the chess framework provided by [Sebastian Lague](https://github.com/SebLague/Chess-Challenge).  
It includes two chess bots developed by me:  
- **MyBot** – Utilizes **Alpha-Beta pruning** for decision-making.  
- **EvilBot** – An experimental bot that uses **Monte Carlo Tree Search (MCTS)**.  

## Installation  

1. Install the [.NET Runtime 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).  
2. Download the pre-built project from the [Releases](https://github.com/wilfre-oss/chess/releases) section **or** clone the repository and build from source.  

## Usage  

### Running the Pre-Built Version  
1. Extract the downloaded archive.  
2. Run the `.exe` file.  

### Running from Source  
1. Open the project in an IDE that supports `.csproj` files, such as [Visual Studio](https://visualstudio.microsoft.com/downloads/).  
2. Open `chess-challenge.csproj`.  
3. Build and run the project.  

## Bot Details  

- **MyBot** – Implements **Alpha-Beta Pruning**, a depth-first search algorithm that optimizes move selection.  
- **EvilBot** – Uses **Monte Carlo Tree Search (MCTS)**, a probabilistic approach that simulates games to improve move choices.  

