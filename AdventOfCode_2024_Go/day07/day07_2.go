package day07

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func concat(a, b int64) int64 {
	multiplier := int64(1)
	for temp := b; temp > 0; temp /= 10 {
		multiplier *= 10
	}
	return a*multiplier + b
}

func calculate2(expected, valueSoFar int64, eqparams []int64, index int) bool {
	if valueSoFar > expected {
		return false
	}
	if index == len(eqparams) {
		return expected == valueSoFar
	}
	return calculate2(expected, valueSoFar+eqparams[index], eqparams, index+1) ||
		calculate2(expected, valueSoFar*eqparams[index], eqparams, index+1) ||
		calculate2(expected, concat(valueSoFar, eqparams[index]), eqparams, index+1)
}

func Executepart2() int64 {
	var result int64 = 0

	var fileName string = "./day07/day07.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for index := 0; index < len(fileContent); index++ {
			expected := utilities.StringToInt64(strings.Split(fileContent[index], ":")[0])
			values := make([]int64, 0)
			for _, val := range strings.Split(strings.Split(fileContent[index], ":")[1], " ") {
				values = append(values, utilities.StringToInt64(val))
			}
			if calculate2(expected, 0, values, 0) {
				result += expected
			}
		}
	}

	return result
}
