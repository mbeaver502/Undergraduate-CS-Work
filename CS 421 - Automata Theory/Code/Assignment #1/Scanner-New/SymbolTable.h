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

		// Constructors and Destructor
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