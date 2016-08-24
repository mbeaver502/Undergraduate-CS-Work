#include <math.h>
#include <string.h>
#include "SymbolTable.h"


//-------------------------------------------------------------------------
// SymbolTable
//-------------------------------------------------------------------------


/*
*/
SymbolTable::SymbolTable() {

    tableSize = 0;
    table = 0;
    slotsOpen = false;
    originalNumKeys = 0;

	initVal.key = -1;
	initVal.code = -1;
	
}


/*
*/
SymbolTable::SymbolTable(const int numKeys) : originalNumKeys(numKeys) {

	// In case of invalid value for numKeys
	if (numKeys < 0) {

		table = 0;
		tableSize = 0;
		slotsOpen = false;

		return;

	}

	// For any values of numKeys > 0
	if (numKeys != 0) {

		// Double the number of keys
		int temp = (numKeys * 2);

		// Then find a prime number (hopefully close by) to set as tableSize
		while (!isPrime(temp))
			temp++;

		tableSize = temp;

	}

	else
		tableSize = 1;

	table = new Record[tableSize];

	this->initVal.key = -1;
	this->initVal.code = -1;

	// Initialize all slots to this->initVal (not taken)
	for (int i = 0; i < tableSize; i++)
		table[i] = this->initVal;

	slotsOpen = true;

}


/*
*/
SymbolTable::SymbolTable(const SymbolTable & source) {

	// If there is a source hash table, create a new hash table copy the source values
	if (source.table != 0) {

		this->tableSize = source.tableSize;
		this->slotsOpen = source.slotsOpen;

		this->table = new Record[tableSize];

		for (int i = 0; i < this->tableSize; i++)
			this->table[i] = source.table[i];

	}

	// Otherwise, set this hash table to NULL
	else {

		this->tableSize = 0;
		this->slotsOpen = false;
		this->table = 0;

	}

}


/*
*/
SymbolTable::~SymbolTable() {

	// Return table memory
	if (table != 0)
		delete[] table;


	// Set back to default -- probably superfluous
	table = 0;
	tableSize = 0;
	slotsOpen = false;

}


/*
*/
SymbolTable & SymbolTable::operator =(const SymbolTable & source) {

	// Check for self-assignment
	if (this == &source)
		return *this;

	// If there is a source hash table, create a new hash table copy the source values
	if (source.table != 0) {

		this->tableSize = source.tableSize;
		this->slotsOpen = source.slotsOpen;

		if (this->table == 0)
			this->table = new Record[tableSize];

		for (int i = 0; i < this->tableSize; i++)
			this->table[i] = source.table[i];

	}

	// Otherwise, set this hash table to NULL
	else {

		this->tableSize = 0;
		this->slotsOpen = false;
		this->table = 0;

	}

	return *this;

}


/*
*/
bool SymbolTable::isPrime(const int val) const {

	// Simple trial division
	int limit = int(sqrt(double(val)));

	for (int i = 2; i <= limit; i++) {

		// Return false if val is a composite number
		if (val % i == 0)
			return false;

	}

	return true;

}


/*
*/
int SymbolTable::NewSlot(const unsigned int HashVal, int & tryNumber) const  {

	unsigned int location = HashVal;

	// Loop through the hash table until an available slot is found
	while ((table[location]).key != (this->initVal).key) {

		// If the table is full, return -1
		if (tryNumber == tableSize)
			return -1;

		location++;
		location %= tableSize;

		tryNumber++;

	}

	return location;

}


/*
*/
bool SymbolTable::querySlotsOpen() const {

	return slotsOpen;

}


/*
*/
// http://www.eternallyconfuzzled.com/tuts/algorithms/jsw_tut_hashing.aspx
unsigned int SymbolTable::doHash(const char name[]) const {

	unsigned int hash = 0;
	unsigned int len = strlen(name);

	for (unsigned int i = 0; i < len; i++) {

		hash += name[i];
		hash += (hash << 10);
		hash ^= (hash >> 6);

	}

	hash += (hash << 3);
	hash ^= (hash >> 11);
	hash += (hash << 15);

	return hash;

}


/*
*/
int SymbolTable::areOpenSlots() {

	// Check for open (-1) slots
	for (int i = 0; i < tableSize; i++) {

		if (table[i].code == (this->initVal).code) {

			slotsOpen = true;
			return i;

		}

	}

	slotsOpen = false;

	return -1;

}


/*
*/

// ******************** UPDATE RECORD's KEY ON INSERT ********************
int SymbolTable::Insert(const Record key) {

	unsigned int hashVal;
	int numSlots = 0;

	if (!slotsOpen) 
		return -1;

	// Calculate the hash
	hashVal = doHash(key.name) % tableSize;

	
	if (((table[hashVal]).key == (this->initVal).key) &&
		(strcmp((table[hashVal]).name, (this->initVal).name) == 0) &&
		((table[hashVal]).code == (this->initVal).code)) {

		table[hashVal] = key;
		(table[hashVal]).key = int(hashVal);

		numSlots++;

	}
	

	// Try again, using linear probing
	else {

		numSlots++;
		int slot = NewSlot(hashVal, numSlots);

		

		// Found an available slot
		if ((table[slot]).key == (this->initVal).key) {

			table[slot] = key;
			(table[slot]).key = int(slot);

			Record x;
			x = table[slot];
			x = x;

		}

		else
			slotsOpen = false;

	}

	return numSlots;

}


/*
*/
bool SymbolTable::Remove(const char name[]) {
	
	unsigned int hashVal;
	int numSlots = 0;

	// No table
	if (table == 0)
		return false;

	// Compute the hash
	hashVal = doHash(name) % tableSize;

	// Try the original hash location
	if (strcmp((table[hashVal]).name, name) == 0) {

		table[hashVal] = this->initVal;
		numSlots++;

		slotsOpen = true;

	}

	// Keep trying until found or not found
	else {

		numSlots++;

		unsigned int location = hashVal;

		// Loop through the entire hash table, if necessary
		while (strcmp((table[location]).name, name) != 0) {

			// Not found
			if (numSlots == tableSize)
				return false;

			else if ((table[location]).key == (this->initVal).key)
				return false;

			location++;
			location %= tableSize;
			numSlots++;

		}

		table[location] = this->initVal;

	}

	return true;

}


/*
*/
int SymbolTable::Search(const char name[]) const  {
	
	unsigned int hashVal;
	int numSlots = 0;
	int location;

	// No table
	if (table == 0)
		return -1;

	// Compute the hash
	hashVal = doHash(name) % tableSize;

	// Try the original hash location
	if (((table[hashVal]).key != (this->initVal).key) && 
		(strcmp((table[hashVal]).name, name) == 0)) {

		location = (table[hashVal]).key;
		numSlots++;

	}
		
	// Otherwise, look through the entire hash table, if necessary
	else {

		numSlots++;

		location = int(hashVal);

		// Loop through the hash table
		while (strcmp((table[location]).name, name) != 0) {

			// Not found
			if (numSlots == tableSize)
				return -1;

			else if ((table[location]).key == (this->initVal).key)
				return -1;

			location++;
			location %= tableSize;
			numSlots++;

		}

	}

	return location;

}


/*
*/
void SymbolTable::ClearTable() {

	// Set all values to this->initVal (available)
	for (int i = 0; i < tableSize; i++)
		table[i] = this->initVal;

	slotsOpen = true;

}


/*
*/
int SymbolTable::getOriginalNumKeys() const {

	return originalNumKeys;

}


int SymbolTable::getTableSize() const {

	return this->tableSize;

}


int SymbolTable::getRecordCode(const char name[]) const {

	int location = this->Search(name);

	if (location < 0)
		return (this->initVal).code;

	else
		return (this->table[location]).code;

}


bool SymbolTable::setRecordCode(const char name[], const int code) {

	int location = this->Search(name);

	if (location < 0)
		return false;

	(this->table[location]).code = code;

	return true;

}






char * SymbolTable::operator [](const int key) const {

	// No table
	if (table == 0)
		return NULL;

	return (this->table[key]).name;

}
