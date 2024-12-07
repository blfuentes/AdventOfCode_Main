package day07

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func calculate1(expected, valueSoFar int64, eqparams []int64) bool {
	if valueSoFar > expected {
		return false
	}
	if len(eqparams) == 0 {
		return expected == valueSoFar
	}
	return calculate1(expected, valueSoFar+eqparams[0], eqparams[1:]) ||
		calculate1(expected, valueSoFar*eqparams[0], eqparams[1:])
}

func Executepart1() int64 {
	var result int64 = 0

	var fileName string = "./day07/day07.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for index := 0; index < len(fileContent); index++ {
			expected := utilities.StringToInt64(strings.Split(fileContent[index], ":")[0])
			values := make([]int64, 0)
			for _, val := range strings.Split(strings.Split(fileContent[index], ":")[1], " ") {
				values = append(values, utilities.StringToInt64(val))
			}
			if calculate1(expected, 0, values) {
				result += expected
			}
		}
	}

	return result
}
