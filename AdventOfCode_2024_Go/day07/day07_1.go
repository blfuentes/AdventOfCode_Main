package day07

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day07/day07.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
