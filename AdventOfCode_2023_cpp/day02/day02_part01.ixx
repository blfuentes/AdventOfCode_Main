#include <vector>
#include <string>
#include <regex>
#include <iostream>
#include <sstream>

import Utilities;

using namespace std;

export module day02_part01;

namespace day02_part01
{
	class GameCube
	{
	public:
		GameCube(int numOfCubes, string color)
			: _numOfCubes(numOfCubes), _color(color)
		{
		}

		int GetNumOfCubes() const
		{
			return _numOfCubes;
		}

		string GetColor() const
		{
			return _color;
		}
	private:
		int _numOfCubes;
		string _color;
	};

	class GameSet
	{
	public:
		GameSet(int idGame, vector<vector<GameCube>> gameCubes)
			: idGame(idGame), gameCubes(gameCubes)
		{
		}

		int GetIdGame() const
		{
			return idGame;
		}

		vector<vector<GameCube>> GetGameCubes() const
		{
			return gameCubes;
		}
	private:
		int idGame;
		vector<vector<GameCube>> gameCubes;
	};

	GameSet buildGameSet(string line)
	{
		int gameId = 0;
		vector<vector<GameCube>> gameCubes;

		// extract game part
		std::istringstream gameParts(line);
		std::vector<std::string> parts;
		std::string part;
		while (std::getline(gameParts, part, ' ')) {
			parts.push_back(part);
		}
		gameId = stoi(parts[1]);

		// extract cubes part
		regex rgx("(( \\d+ (blue|green|red),\\s*)+)?( \\d+ (blue|green|red)\\s*)");
		std::sregex_iterator iter(line.begin(), line.end(), rgx);
		std::sregex_iterator end;

		while (iter != end) 
		{
			std::istringstream cubeSet(iter->str());
			std::string cube;
			std::vector<GameCube> cubesOfCubSet;
			while (std::getline(cubeSet, cube, ',')) 
			{
				std::istringstream partDef(cube);
				string numOfCubes;
				string color;
				partDef >> numOfCubes >> color;
				cubesOfCubSet.push_back(GameCube(stoi(numOfCubes), color));
			}
			
			// Move to the next match
			++iter;
			gameCubes.push_back(cubesOfCubSet);
		}

		return GameSet(gameId, gameCubes);
	}

	export int Execute()
	{
		/*vector<string> lines = Utilities::readTextFile("day02/day02_input.txt");*/
		vector<string> lines = Utilities::readTextFile("day02/test_input_01.txt");
		int result = 0;

		for (string line : lines)
		{
			GameSet gameSet = buildGameSet(line);
		}

		return 0;
	}
}