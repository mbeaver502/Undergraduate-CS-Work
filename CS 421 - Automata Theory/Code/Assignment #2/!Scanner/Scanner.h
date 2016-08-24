/*
Michael Beaver
CS 421 - Automata Theory & Compiler Construction
Assignment #1 - MINI-P Scanner
20 March 2015
Program Description: 
	This program implements a Scanner for a MINI-P compiler. This program emulates
	the basic function of a Parser as it would continuously prompt the Scanner to
	scan tokens in the buffer. This implementation assumes that a source file is
	being read line-by-line. That is, the lookahead buffer is assumed to be a 
	single line rather than a continuous stream of bytes. This could easily be
	adjusted if necessary.
*/

#ifndef SCANNER_H
#define SCANNER_H

#include <iostream>
#include "SymbolTable.h"
#include "Constants.h"

//--------------------------------------------------------------------------

class Scanner {
      
    private:
              
        bool isAlpha(char c) const;
        bool isDigit(char c) const;
        bool isDelimiter(char c) const;

		int recognizeKeyword(const std::string & str, int ptrPos, int & delta) const;
		int recognizeOperator(const std::string & str, int ptrPos, int & delta) const;
		bool recognizeIdentifier(const std::string & str, int ptrPos, int & delta) const;
		int recognizeDelimiter(const std::string & str, int ptrPos, int & delta) const;
		bool recognizeInteger(const std::string & str, int ptrPos, int & delta) const;
		int match(const std::string & str, const std::string & token, int cPtr, int lPtr) const;   
		
		
    public:    
      
        int scan(const std::string & s, int & ptr, int & numErrors, SymbolTable * ST) const;
      
};

#endif
