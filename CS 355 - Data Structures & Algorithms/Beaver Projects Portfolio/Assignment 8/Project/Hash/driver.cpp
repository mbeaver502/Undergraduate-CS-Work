/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 8, Homework 6
	Description: This program implements a simple templated HashTable data structure.  For the sake of simplicity,
	             a separate Slot object is not used.  Rather, data is stored in a dynamic array, and a value of
				 -1 indicates that a slot is available to use.  For a more robust solution, a dynamic array of
				 Slot objects should be used.
    Due Date: October 23, 2012


	TO DO: USE STATIC MEMBER FOR INITVAL INSTEAD OF SLOT_OPEN = -1 CONSTANT
	TO DO: DISALLOW DUPLICATE INPUTS


*/


#include <iostream>
#include <string>
#include <time.h>
#include "HashTable.h"


using namespace std;


#define DASHES cout << "------------------------------------" << endl;

void PrintMenu();

void testInsert(HashTable<int> & HT);
void testRemove(HashTable<int> & HT);
void testSearch(const HashTable<int> & HT);

int main() {

	srand(time(NULL));

	HashTable<int> myHash1(10, -1);
	HashTable<int> myHash2(50, -1);
	HashTable<int> myHash3(100, -1);
	HashTable<int> myHash4(1000, -1);
	HashTable<int> myHash5(10000, -1);

	cout << "INSERT TEST WITH DATA SET SIZE 10: " << endl;
	DASHES;
	testInsert(myHash1);

	system("PAUSE");
	system("CLS");

	cout << "SEARCH TEST WITH DATA SET SIZE 10: " << endl;
	DASHES;
	testSearch(myHash1);

	system("PAUSE");
	system("CLS");

	cout << "REMOVE TEST WITH DATA SET SIZE 10: " << endl;
	DASHES;
	testRemove(myHash1);

	system("PAUSE");
	system("CLS");

	cout << "INSERT TEST WITH DATA SET SIZE 50: " << endl;
	DASHES;
	testInsert(myHash2);

	system("PAUSE");
	system("CLS");

	cout << "SEARCH TEST WITH DATA SET SIZE 50: " << endl;
	DASHES;
	testSearch(myHash2);

	system("PAUSE");
	system("CLS");

	cout << "REMOVE TEST WITH DATA SET SIZE 50: " << endl;
	DASHES;
	testRemove(myHash2);

	system("PAUSE");
	system("CLS");

	cout << "INSERT TEST WITH DATA SET SIZE 100: " << endl;
	DASHES;
	testInsert(myHash3);

	system("PAUSE");
	system("CLS");

	cout << "SEARCH TEST WITH DATA SET SIZE 100: " << endl;
	DASHES;
	testSearch(myHash3);

	system("PAUSE");
	system("CLS");

	cout << "REMOVE TEST WITH DATA SET SIZE 100: " << endl;
	DASHES;
	testRemove(myHash3);

	system("PAUSE");
	system("CLS");

	cout << "INSERT TEST WITH DATA SET SIZE 1000: " << endl;
	DASHES;
	testInsert(myHash4);

	system("PAUSE");
	system("CLS");

	cout << "SEARCH TEST WITH DATA SET SIZE 1000: " << endl;
	DASHES;
	testSearch(myHash4);

	system("PAUSE");
	system("CLS");

	cout << "REMOVE TEST WITH DATA SET SIZE 1000: " << endl;
	DASHES;
	testRemove(myHash4);

	system("PAUSE");
	system("CLS");

	cout << "INSERT TEST WITH DATA SET SIZE 10000: " << endl;
	DASHES;
	testInsert(myHash5);

	cout << "SEARCH TEST WITH DATA SET SIZE 10000: " << endl;
	DASHES;
	testSearch(myHash5);

	system("PAUSE");
	system("CLS");

	cout << "REMOVE TEST WITH DATA SET SIZE 10000: " << endl;
	DASHES;
	testRemove(myHash5);

	system("PAUSE");
	return 0;

}


void testInsert(HashTable<int> & HT) {

	int numKeys = HT.getOriginalNumKeys();
	int temp = rand() % numKeys;
	temp++;

	for (int i = 0; i < numKeys; i++) {

		cout << "INSERTING " << temp << "\t. . . Hit: ";
		cout << HT.Insert(temp) << "\tslot(s)" << endl;

		temp = rand() % numKeys;
		temp++;

	}

}

void testRemove(HashTable<int> & HT) {

	int numKeys = HT.getOriginalNumKeys();
	int temp = rand() % numKeys;
	temp++;

	for (int i = 0; i < numKeys; i++) {

		cout << "REMOVING " << temp << "\t. . . Hit: ";
		cout << HT.Remove(temp) << "\tslot(s)" << endl;

		temp = rand() % numKeys;
		temp++;

	}

}

void testSearch(const HashTable<int> & HT) {

	int numKeys = HT.getOriginalNumKeys();
	int temp = rand() % numKeys;
	temp++;

	for (int i = 0; i < numKeys; i++) {

		cout << "FINDING " << temp << "\t. . . Hit: ";
		cout << HT.Search(temp) << "\tslot(s)" << endl;

		temp = rand() % numKeys;
		temp++;

	}

}


