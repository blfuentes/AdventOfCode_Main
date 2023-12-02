#include <fstream>
#include <vector>
#include <string>

using namespace std;

export module Utilities;

namespace Utilities
{
	// function to read the content of text file
    export vector<string> readTextFile(const string& filePath) {
        vector<string> lines;
        ifstream inFile(filePath);
        if (inFile.is_open()) {
            string line;
            while (getline(inFile, line)) {
                lines.push_back(line);
            }
            inFile.close();
        }
        return lines;
    }
}