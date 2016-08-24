/*
Michael Beaver
CS 421 - Automata Theory & Compiler Construction
Assignment #2 - MINI-P Parser
27 April 2015
Program Description:
	This program implements a parser for a MINI-P compiler. This program emulates
	the basic function of a parser as it would continuously prompt the Scanner to
	scan tokens in the buffer. Special note: This implementation assumes all
	executable statements (i.e., assign, read, write) end with a semicolon. This
	implementation also will stop trying to parse if it encounters a bad statement.
	Any statements after the initial bad statement will not be parsed (correctly),
	but the bad statement will be identified. In this case, the parser will also
	attempt to parse the END. keyword, which will obviously fail.
*/

#ifndef PARSER_H
#define PARSER_H

#include <iostream>

#include "Constants.h"
#include "SymbolTable.h"
#include "Scanner.h"

class Parser {

	private:

		SymbolTable ST;
		Scanner SC;
		int numErrors;
		int numLine;
		int numLines;
		int ptr;
		std::string buffer;
		int tc;

		void advance();
	
		bool parseProgName();
		bool parseDecList();
		bool parseDec();
		bool parseType();
		bool parseIdList();
		bool parseStmtList();
		bool parseStmt();
		bool parseAssign();
		bool parseExp();
		bool parseTerm();
		bool parseFactor();
		bool parseRead();
		bool parseWrite();
		bool parseFor();
		bool parseIndexExp();
		bool parseBody();

		void toUpper(std::string & str);
		void trimSpaces(std::string & str);
		void preProcess(std::string & str, bool & commentFlag);
		void detectComment(std::string & str, bool & commentFlag);

		void initST();
		void printST();

	public:

		Parser();
	
		void init(const std::string & filename);

		bool parseProg();

		int getNumErrors() const;
		int getNumLines() const;

};

#endif
