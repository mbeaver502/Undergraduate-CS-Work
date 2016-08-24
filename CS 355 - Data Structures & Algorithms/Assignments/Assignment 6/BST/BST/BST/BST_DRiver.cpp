/*
	Names: Drew Aaron, Michael Beaver, Andrew Hamilton, and Elizabeth McClellan
	CS355 Fall 2012
	Assignment: Assignment 6, Homework 4 -- Group 2
	Description: This program implements and tests an integer-based Binary Search Tree.  The BST
				 has the ability to insert values, remove values, copy and assign itself to other
				 BSTs, print itself in three ways (Pre, Post, and In order), and manipulate the
				 position of the cursor.
    Due Date: October 9, 2012
*/


#include "BST.h"
#include <iostream>
#include <string>
using namespace std;


//------------------------------------------------------


void PrintMenu();

void behemothTest();
void behemothPart1(BST & b);
void behemothPart2(BST & b);
void behemothPart3(BST & b);
void behemothPart4(BST & b);
void behemothPart5(BST & b);
void behemothPart6(BST & b);
void behemothPart7(BST & b);


//------------------------------------------------------


int main(){
	
	BST testBST;
	BST testAssign;
	BNode* temp;
	int data;

	PrintMenu();
	cout << "-->";
	char choice;
	cin >> choice;

	while(choice != 'q' && choice != 'Q'){
		
		if (choice == '!'){
			BST testCopy(testBST);
			cout << "Result:" << "Print New Copy" << endl;
			testCopy.PrintIn(cout); cout << endl;
			testCopy.Insert(-10000);
			cout << "Result: " << "Print Modified Copy" << endl;
			testCopy.PrintIn(cout); cout << endl;
			cout << "Result: " << "Print Original Test List" << endl;
			testBST.PrintIn(cout); cout << endl;
		}
		else{
			switch (choice){
				case 'h':
				case 'H': PrintMenu(); break;
				case '+':
					cin >> data;
					testBST.Insert(data);
					break;		
				case '-':
					cin >> data;
					if (testBST.Remove(data) == false)
						cout << "Data not found in the tree!" << endl;
					break;
				
				case '@':
					cout <<"Result:";
					temp = testBST.AtCursor();
					if (temp)
						cout << temp->GetData() << endl;
					else
						cout << "NULL POINTER" << endl;
					break;
				case '<':
					testBST.GoToBeginning();
					break;
				case '>':
					testBST.GoToEnd();
					break;
				case 'n':
				case 'N':
					testBST.GoToNext();
					break;
				case 'p':
				case 'P':
					testBST.GoToPrev();
					break;
				case '^':
					testBST.GoToRoot();
					break;
				case 'c':
				case 'C':
					testBST.ClearList();
					break;
					
				case 'e':
				case 'E':
					if (testBST.Empty())
						cout <<"Result:" <<  "List Is Empty" << endl;
					else
						cout <<"Result:" <<  "List is Not Empty" << endl;
					break;
				case '#':
					//assign list
					testAssign = testBST;
					testAssign.Insert(-100000);
					cout << "Modify New List" << endl;
					testAssign.PrintIn(cout); cout << endl;
					cout << "Old List should not be affected" << endl;
					testBST.PrintIn(cout); cout << endl;
					testAssign.~BST();
					cout << "Destroy New List" << endl;
					cout << "Old List should not be affected" << endl;
					testBST.PrintIn(cout); cout << endl;
					break;
				case '?':
					cin >> data;
					if (testBST.Find(data) != NULL)
						cout << "Result:" << data << "\tfound" << endl;
					else
						cout << "Result:" << data << "\tnot found" << endl;
					break;
				case 'i':
				case 'I':
					cout << "Print INORDER" << endl;
					testBST.PrintIn(cout); cout << endl;
					break;
				case 'r':
				case 'R':
					cout << "Print PREORDER" << endl;
					testBST.PrintPre(cout); cout << endl;
					break;
				case 'o':
				case 'O':
					cout << "Print POSTORDER" << endl;
					testBST.PrintPost(cout); cout << endl;
					break;

				case 'b':
				case 'B':
					// UNLEASH THE BEHEMOTH
					behemothTest();
					break;

				default:
					cout << "INVALID CHOICE, Choose Again" << endl;
			}

		}
		testBST.PrintIn(cout); cout << endl;
		cout << "-->";
		cin >> choice;
	}

	return 0;
}

void PrintMenu(){
	/*Commands borrowed from Lab*/
	cout << endl << endl;
	cout << "  Command Line Options" << endl;
	cout << "  H\tHelp: Show Command Line Options" << endl;
	cout << "  +x\tInsert x in order" << endl;
	cout << "  -x\tRemove x" << endl;
	cout << "  @   : Display the data item marked by the cursor" << endl;
    cout << "  <   : Go to the beginning of the list" << endl;
    cout << "  >   : Go to the end of the list" << endl;
    cout << "  N   : Go to the next data item" << endl;
    cout << "  P   : Go to the prior data item" << endl;
	cout << "  ^   : Go to Root item" << endl;
    cout << "  C   : Clear the list" << endl;
    cout << "  E   : Empty list?" << endl;
    cout << "  !   : Test copy constructor" << endl;
    cout << "  #   : Test assignment operator, ClearList must work first before testing" << endl;
    cout << "  ?x  : Search rest of list for x " << endl;
	cout << "  I   : Print the List InOrder" << endl;
	cout << "  R   : Print the List PreOrder" << endl;
	cout << "  O   : Print the List PostOrder" << endl;
	cout << "  B   : UNLEASH THE BEHEMOTH" << endl;
    cout << "  Q   : Quit the test program" << endl;
    cout << endl << endl << endl;
	
}


/*
  Name: behemothPart1   
  Writer(s): Michael
  Purpose: Tests BST insertion. 
  Incoming: b is the BST that is to be tested and operated upon
  Outgoing: b is the BST that is to be tested and operated upon
  Return: N/A (void)
*/
void behemothPart1(BST & b) {

	// INSERTION
	cout << "Inserting 50 . . . ";
	if (b.Insert(50))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 48 . . . ";
	if (b.Insert(48))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 21 . . . ";
	if (b.Insert(21))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 63 . . . ";
	if (b.Insert(63))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 13 . . . ";
	if (b.Insert(13))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 32 . . . ";
	if (b.Insert(31))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 45 . . . ";
	if (b.Insert(45))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 71 . . . ";
	if (b.Insert(71))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 61 . . . ";
	if (b.Insert(61))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 55 . . . ";
	if (b.Insert(55))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 62 . . . ";
	if (b.Insert(62))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 19 . . . ";
	if (b.Insert(19))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 65 . . . ";
	if (b.Insert(65))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 69 . . . ";
	if (b.Insert(69))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 80 . . . ";
	if (b.Insert(80))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 32 . . . ";
	if (b.Insert(32))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 60 . . . ";
	if (b.Insert(60))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 09 . . . ";
	if (b.Insert(9))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 66 . . . ";
	if (b.Insert(66))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 40 . . . ";
	if (b.Insert(40))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 64 . . . ";
	if (b.Insert(64))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 46 . . . ";
	if (b.Insert(46))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 75 . . . ";
	if (b.Insert(75))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 77 . . . ";
	if (b.Insert(77))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	cout << "Inserting 49 . . . ";
	if (b.Insert(49))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;


}


/*
  Name: behemothPart2   
  Writer(s): Michael
  Purpose: Tests BST print operations.
  Incoming: b is the BST that is to be tested and operated upon
  Outgoing: b is the BST that is to be tested and operated upon
  Return: N/A (void)
*/
void behemothPart2(BST & b) {

	cout << "PRINT IN ORDER:" << endl;
	b.PrintIn(cout);
	cout << endl << endl;

	cout << "PRINT PRE-ORDER:" << endl;
	b.PrintPre(cout);
	cout << endl << endl;

	cout << "PRINT POST-ORDER:" << endl;
	b.PrintPost(cout);
	cout << endl << endl;


}


/*
  Name: behemothPart3  
  Writer(s): Michael
  Purpose: Tests GoToNext, GoToPrev, and GoToBeginning.
  Incoming: b is the BST that is to be tested and operated upon
  Outgoing: b is the BST that is to be tested and operated upon
  Return: N/A (void)
*/
void behemothPart3(BST & b) {

	cout << "GO TO BEGINNING:" << endl;
	b.GoToBeginning();
	b.PrintIn(cout);
	cout << endl << endl;
	system("PAUSE");

	for (int i = 0; i < 25; i++) {
	
		cout << endl << "GO TO NEXT:" << endl;
		b.GoToNext();
		b.PrintIn(cout);
		cout << endl << endl;
		system("PAUSE");

	}

	cout << endl;

	for (int i = 0; i < 26; i++) {

		cout << endl << "GO TO PREVIOUS:" << endl;
		b.GoToPrev();
		b.PrintIn(cout);
		cout << endl << endl;
		system("PAUSE");

	}

	cout << endl;

	cout << "GO TO ROOT:" << endl;
	b.GoToRoot();
	b.PrintIn(cout);
	cout << endl << endl;

}


/*
  Name: behemothPart4 
  Writer(s): Michael
  Purpose: Tests Find, AtCursor, and GetData.
  Incoming: b is the BST that is to be tested and operated upon
  Outgoing: b is the BST that is to be tested and operated upon
  Return: N/A (void)
*/
void behemothPart4(BST & b) {

	BNode * temp = NULL;

	cout << "At cursor: " << b.AtCursor()->GetData() << endl << endl;

	system("PAUSE");
	cout << endl;
	  
	temp = b.Find(31);
	cout << "Searching for 31 . . . ";
	if (temp != NULL)
		cout << "SUCCESS! @ " << temp << ": " << temp->GetData() << endl;
	else
		cout << "FAILED!" << endl;

	cout << endl << "At cursor: " << b.AtCursor()->GetData() << endl << endl;

	system("PAUSE");
	cout << endl;

	temp = b.Find(46);
	cout << "Searching for 46 . . . ";
	if (temp != NULL)
		cout << "SUCCESS! @ " << temp << ": " << temp->GetData() << endl;
	else
		cout << "FAILED!" << endl;

	cout << endl << "At cursor: " << b.AtCursor()->GetData() << endl << endl;

	system("PAUSE");
	cout << endl;

	temp = b.Find(40);
	cout << "Searching for 40 . . . ";
	if (temp != NULL)
		cout << "SUCCESS! @ " << temp << ": " << temp->GetData() << endl;
	else
		cout << "FAILED!" << endl;

	cout << endl << "At cursor: " << b.AtCursor()->GetData() << endl << endl;

}


/*
  Name: behemothPart5
  Writer(s): Michael
  Purpose: Tests BST assignment.
  Incoming: b is the BST that is to be tested and operated upon
  Outgoing: b is the BST that is to be tested and operated upon
  Return: N/A (void)
*/
void behemothPart5(BST & b) {

	BST assignedBST;

	cout << "ASSIGNING BST . . . " << endl;
	assignedBST = b;

	cout << "ORIGINAL:" << endl;
	b.PrintIn(cout);

	cout << endl << endl << "NEW:" << endl;
	assignedBST.PrintIn(cout);

	cout << endl << endl << "MODIFYING NEW BST . . . " << endl;
	assignedBST.Insert(5000);
	assignedBST.Insert(-5000);

	cout << "ORIGINAL:" << endl;
	b.PrintIn(cout);

	cout << endl << endl << "MODIFIED NEW:" << endl;
	assignedBST.PrintIn(cout);

	cout << endl << endl << "DESTROYING NEW BST . . . " << endl;
	assignedBST.~BST();

	cout << "ORIGINAL:" << endl;
	b.PrintIn(cout);

	cout << endl << endl << "GO TO NEXT:" << endl;
	b.GoToNext();
	b.PrintIn(cout);
	cout << endl << endl;

}


/*
  Name: behemothPart6   
  Writer(s): Michael
  Purpose: Tests BST copy constructor. 
  Incoming: b is the BST that is to be tested and operated upon
  Outgoing: b is the BST that is to be tested and operated upon
  Return: N/A (void)
*/
void behemothPart6(BST & b) {

	cout << "COPYING BST . . . " << endl;
	BST copyBST = b;

	cout << "ORIGINAL:" << endl;
	b.PrintIn(cout);

	cout << endl << endl << "NEW:" << endl;
	copyBST.PrintIn(cout);

	cout << endl << endl << "MODIFYING NEW BST . . . " << endl;
	copyBST.Insert(3000);
	copyBST.Insert(-3000);
	copyBST.Insert(47);

	cout << "ORIGINAL:" << endl;
	b.PrintIn(cout);

	cout << endl << endl << "MODIFIED NEW:" << endl;
	copyBST.PrintIn(cout);

	cout << endl << endl << "DESTROYING NEW BST . . . " << endl;
	copyBST.~BST();

	cout << "ORIGINAL:" << endl;
	b.PrintIn(cout);

	cout << endl << endl << "GO TO PREVIOUS:" << endl;
	b.GoToPrev();
	b.PrintIn(cout);
	cout << endl << endl;

	cout << "GO TO ROOT:" << endl;
	b.GoToRoot();
	b.PrintIn(cout);
	cout << endl << endl;

	BNode * temp = NULL;

	temp = b.Find(20);
	cout << "Searching for 20 . . . ";
	if (temp != NULL)
		cout << "SUCCESS! @ " << temp << ": " << temp->GetData() << endl;
	else
		cout << "FAILED!" << endl;

}


/*
  Name: behemothPart7  
  Writer(s): Michael
  Purpose: Tests BST Remove.
  Incoming: b is the BST that is to be tested and operated upon
  Outgoing: b is the BST that is to be tested and operated upon
  Return: N/A (void)
*/
void behemothPart7(BST & b) {

	cout << "REMOVING 19 . . . ";
	if (b.Remove(19))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	b.PrintIn(cout);
	cout << endl << endl;

	cout << "GO TO NEXT:" << endl;
	b.GoToNext();
	b.PrintIn(cout);
	cout << endl << endl;

	cout << "REMOVING 69 . . . ";
	if (b.Remove(69))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	b.PrintIn(cout);
	cout << endl << endl;

	cout << "REMOVING 59 . . . ";
	if (b.Remove(59))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	b.PrintIn(cout);
	cout << endl << endl;

	cout << "REMOVING 50 . . . ";
	if (b.Remove(50))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	b.PrintIn(cout);
	cout << endl << endl;

	cout << "REMOVING ALL BUT ROOT . . . " << endl;
	b.Remove(9);
	b.Remove(13);
	b.Remove(21);
	b.Remove(31);
	b.Remove(32);
	b.Remove(40);
	b.Remove(45);
	b.Remove(46);
	b.Remove(48);
	b.Remove(49);
	b.Remove(60);
	b.Remove(61);
	b.Remove(62);
	b.Remove(63);
	b.Remove(64);
	b.Remove(65);
	b.Remove(66);
	b.Remove(71);
	b.Remove(75);
	b.Remove(77);
	b.Remove(80);

	cout << "AFTER REMOVAL:" << endl;
	b.PrintIn(cout);
	cout << endl << endl;

	cout << "Inserting 69 . . . ";
	if (b.Insert(69))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	b.PrintIn(cout);
	cout << endl << endl;

	cout << "Inserting 10 . . . ";
	if (b.Insert(10))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILED!" << endl;

	b.PrintIn(cout);
	cout << endl << endl;

	cout << "CLEARING TREE . . . " << endl;
	b.ClearList();
	b.PrintIn(cout);
	cout << endl << endl;

}


/*
  Name: behemothTest   
  Writer(s): Michael
  Purpose: Combines all behemothPartXs into one successive test.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void behemothTest() {

	system("CLS");
	cout << "================================" << endl;
	cout << "|         THE BEHEMOTH         |" << endl;
	cout << "================================" << endl << endl;

	BST testBST;
	
	behemothPart1(testBST);
	cout << endl;
	system("PAUSE");
	cout << endl;

	behemothPart2(testBST);
	cout << endl;
	system("PAUSE");
	cout << endl;

	behemothPart3(testBST);
	cout << endl;
	system("PAUSE");
	cout << endl;

	behemothPart4(testBST);
	cout << endl;
	system("PAUSE");
	cout << endl;

	behemothPart5(testBST);
	cout << endl;
	system("PAUSE");
	cout << endl;

	behemothPart6(testBST);
	cout << endl;
	system("PAUSE");
	cout << endl;

	behemothPart7(testBST);
	cout << endl;
	system("PAUSE");
	cout << endl;

	PrintMenu();

}