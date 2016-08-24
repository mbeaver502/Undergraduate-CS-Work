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

#ifndef SYMBOLTABLE_H
#define SYMBOLTABLE_H

#include "Constants.h"

//--------------------------------------------------------------------------

class SymbolTable {

	private:

		int originalNumKeys;
		int tableSize;
		Record * table;
		bool slotsOpen;
		Record initVal;

		bool isPrime(const int val) const;
		int NewSlot(const unsigned int HashVal, int & tryNumber) const;
		bool querySlotsOpen() const;
		unsigned int doHash(const char name[]) const;


	public:

		SymbolTable();
		SymbolTable(const int numKeys);
		SymbolTable(const SymbolTable & source);
		~SymbolTable();

		SymbolTable& operator =(const SymbolTable & source);
		char * operator [](const int key) const;

		int areOpenSlots();
		int Insert(const Record key);
		bool Remove(const char name[]);
		int Search(const char name[]) const;
		void ClearTable();

		int getOriginalNumKeys() const;
		int getTableSize() const;
		int getRecordCode(const char name[]) const;
		bool setRecordCode(const char name[], const int code);

};

#endif
