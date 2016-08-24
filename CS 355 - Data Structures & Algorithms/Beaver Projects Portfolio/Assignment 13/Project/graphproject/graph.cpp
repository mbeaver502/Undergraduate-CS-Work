/*
	Names: Drew Aaron, Michael Beaver, and Andrew Hamilton
	CS355 Fall 2012
	Assignment: Assignment 13, Group 4
	Description: This program allows a user to create Graph objects, which are composed of Node and Edge
			objects.  The user may manipulate the Graph in various ways, such as adding Nodes.  This program
			features file I/O that allows the user to load data from a formatted file and save a formatted file.
    Due Date: December 4, 2012
*/


#include "graph.h"
#include "minBinaryHeap.h"


// When displaying shortest path, this breaks the path into groups of size GROUP_BREAK
const unsigned int GROUP_BREAK = 1;


/*
  Name: Graph in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Default constructor. Creates a Graph with a valid state.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Graph::Graph() {

	nodeType = "vertices";
	edgeType = "edges";
	weightUnits = "units";
	arr = 0;

}


/*
  Name: Graph in scope of Graph
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Copy constructor.  Allows the user to instantiate a valid Graph object from another Graph object.
  Incoming: g is the source Graph
  Outgoing: N/A
  Return: N/A
*/
Graph::Graph(const Graph & g) {

	// Copy units
	nodeType = g.getNodeType();
	edgeType = g.getEdgeType();
	weightUnits = g.getWeightUnits();

	// Get Nodes
	arr = 0;
	nodeList.clear();
	nodeList.resize(g.getNodeList().size());

	for (unsigned int i = 0; i < g.getNodeList().size(); i++) 
		nodeList[i] = g.getNodeList()[i];
	
}


/*
  Name: ~Graph in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Destructor.  Returns memory and cleans up any data members.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Graph::~Graph() {

	delete [] arr;
	arr = 0;

	int size = nodeList.size();
	for (int i = 0; i < size; i++)
		nodeList[i] = 0;

}


/*
  Name: Overloaded assignment operator in scope of Graph
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Allows the user to set a valid Graph object and all of its properties 
		equal to another Graph object and all of its properties.
  Incoming: g is the source Graph
  Outgoing: N/A
  Return: Returns this Graph with its new properties.
*/
Graph & Graph::operator =(const Graph & g) {
	
	// Self-assignment
	if (this == &g)
		return *this;
	
	// Assign units
	nodeType = g.getNodeType();
	edgeType = g.getEdgeType();
	weightUnits = g.getWeightUnits();

	// Assign Nodes
	arr = 0;
	nodeList.clear();
	nodeList.resize(g.getNodeList().size());
	for (unsigned int i = 0; i < g.getNodeList().size(); i++) 
		nodeList[i] = g.getNodeList()[i];

	return *this;

}


/*
  Name: displayGraph in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Displays information about the graph, as well as all node adjacencies in the graph.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void Graph::displayGraph() const {

	// Display each Node's adjacent Nodes
	int size = nodeList.size();

	cout << "Number of Nodes: " << size << endl;
	cout << "Edge Type: " << edgeType << endl;
	cout << "Node Type: " << nodeType << endl;
	cout << "Weight Units: " << weightUnits << endl << endl;

	if (size != 0) {

		for (int i = 0; i < size; i++) {

			// Single Node
			if (nodeList[i]->getAdjList().size() == 0)
				cout << "[ " << nodeList[i]->getName() << " ]" << endl;

			// Node with adjacent Nodes
			else
				nodeList[i]->displayAdjList();

			cout << endl << endl;

		}

	}

	else
		cout << "Graph contains no Nodes!" << endl;

}


/*
  Name: displayNodes in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Displays each Node and its number of adjacent nodes
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void Graph::displayNodes() const {

	int size = nodeList.size();
	
	if (size != 0) {

		// Display each Node and its number of adjacent Nodes
		for (int i = 0; i < size; i++) {

			cout << nodeList[i]->getName();

			cout << " has " << nodeList[i]->getNumAdjacentNodes() << " adjacent Nodes" << endl << endl;

		}

	}

	else
		cout << "Graph contains no Nodes!" << endl;

}


/*
  Name: isNodeIsolated in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Checks to see if a node is isolated.
  Incoming: n is a pointer to the specified node which is to be checked.
  Outgoing: N/A
  Return: Returns true if the specified node is isolated, false if it is not.
*/
bool Graph::isNodeIsolated(Node * n) const {

	int numNodes = nodeList.size();

	// Loop through all Nodes
	for (int i = 0; i < numNodes; i++) {

		// Get adjacency list for each Node
		int adjSize = nodeList[i]->getNumAdjacentNodes();
		vector<Edge> aList = nodeList[i]->getAdjList();

		// Loop through the adjacency list of each Node
		for (int j = 0; j < adjSize; j++) {

			// If the Node is a source or destination Node in any adjacency list, then it is NOT isolated
			if ((aList[j].getDst() == n) || (aList[j].getSrc() == n))
				return false;
			
		}

	}

	// Node is isolated
	return true;

}


/*
  Name: readfromFile in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: reads data from a file and contructs a graph from said data.
  Incoming: input is the name of the file to open.
  Outgoing: input is the name of the file to open, arr is an array of new Nodes made from input data. 
  Return: N/A
*/
void Graph::readFromFile(string & input) {

	ifstream inFile;
	string line;
	int numNodes;

	inFile.open(input.c_str());

	if(inFile.fail()) {

		cout << "ERROR: Invalid filename!\n";
		return;

	}

	// Get the number of Nodes in the Graph
	getline(inFile, line);
	numNodes = atoi(line.c_str());

	arr = new Node[numNodes];

	// Add default-constructed Nodes into the Graph (modified later)
	for (int i = 0; i < numNodes; i++)
		addNewNode(&arr[i]);

	// Get the Edge type, Node type, and Weight units
	getline(inFile, line);
	setEdgeType(line);
	getline(inFile, line);
	setNodeType(line);
	getline(inFile, line);
	setWeightUnits(line);

	// Get the actual Node names
	for (int i = 0; i < numNodes; i++) {

		 getline(inFile,line);
		 arr[i].setName(line);

	}

	int pos, pos2;
	string srcNode, dstNode, weight;

	// Get the Node adjacencies
	// Format (no spaces): srcNode->dstNode(weight)
	while (!inFile.eof()) {

		// Get the source Node
		getline(inFile, line);

		// Avoid blank lines
		if (line.empty())
			continue;

		pos = line.find("->");
		for (int i = 0; i < pos; i++)
			srcNode += line[i];
		pos += 2;

		// Get the destination Node
		pos2 = line.find("(");
		for (int i = pos; i < pos2; i++)
			dstNode += line[i];	 
		pos2++;

		// Get the Edge weight
		pos = line.find(")");
		for (int i = pos2; i < pos; i++) 
			weight += line[i];

		if (atoi(weight.c_str()) <= 0)
			weight = "1";

		// Find the Nodes in the Node array
		Node * p = findNode(numNodes, srcNode);
		Node * q = findNode(numNodes, dstNode);

		// Nodes not found (probably name mismatch)
		if (p == 0) 
			cout << "ERROR: Node " << srcNode << " NOT found!" << endl;
			
		if (q == 0) 
			cout << "ERROR: Node " << dstNode << " NOT found!" << endl;

		// Complete the adjancency
		else	
			p->addAdjacentNode(q, atoi(weight.c_str()));

		srcNode = dstNode = weight = "";
		 
	 }

	inFile.close();

}


/*
  Name: saveToFile in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Saves the graph to a file in such a way that it can be read from that file and make the same graph
  Incoming: output is the name to give the file
  Outgoing: output is the name to give the file
  Return: N/A
*/
void Graph::saveToFile(string & output) const {

	ofstream outFile;

	// Open output file
	outFile.open(output.c_str());

	// Output number of Nodes and units
	outFile << nodeList.size() << endl;
	outFile << edgeType << endl;
	outFile << nodeType << endl;
	outFile << weightUnits << endl;

	// Output name of Nodes
	for (unsigned int i = 0; i < nodeList.size(); i++) 
		outFile << nodeList[i]->getName() << endl;

	vector<Edge> list;

	// Loop over the Nodes in the Graph
	for (unsigned int i = 0; i < nodeList.size(); i++) {

		list = nodeList[i]->getAdjList();

		// Output the adjacencies for each Node
		for (unsigned int j = 0; j < list.size(); j++) {

			outFile << list[j].getSrc()->getName() << "->";
			outFile << list[j].getDst()->getName() << "(";
			outFile << list[j].getWeight() << ")";
			outFile << endl;

		}

	}

	outFile.close();

}


/*
  Name: addNewNode in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Adds a node to the node list.
  Incoming: n is a pointer to the node to add to the node list.
  Outgoing: N/A
  Return: N/A
*/
void Graph::addNewNode(Node * n) {

	// Push a Node pointer onto the vector of Node pointers
	nodeList.push_back(n);

}


/*
  Name: findNode in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Finds a node whose name matches a specified string
  Incoming: n is the specified string to search for
  Outgoing: N/A
  Return: Returns a pointer to the node with a matching name, or NULL if no node matches the string
*/
Node * Graph::findNode(string n) const {

	int size = nodeList.size();

	// Try to find the Node by name
	for (int i = 0; i < size; i++) {

		if (nodeList[i]->getName() == n)
			return nodeList[i];

	}

	// Not found, return NULL
	return 0;

}


/*
  Name: findNode in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Finds a node whose name matches a specified string within a certain range of nodes
  Incoming: n is the range of nodes to search for the specified string, name is the specified string
  Outgoing: N/A
  Return: Returns a pointer to the node with a matching name, or NULL if no node matches the string
*/
Node * Graph::findNode(int n, string name) const{

	// Looping n times
	for(int i = 0; i < n; i++) {

		// Return if the Node is found by name
		if(arr[i].getName() == name)
			return &arr[i];

	}

	return 0;

}


/*
  Name: dijkstrHeap in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Finds the shortest path from one Node to another using a binary heap.
  Incoming: first is a pointer the node to start at, and last is a pointer to the node to end on
  Outgoing: N/A
  Return: N/A
*/
void Graph::dijkstraHeap(Node * first, Node * last) const {


	if ((first == 0) || (last == 0)) {

		cout << "ERROR: Nodes do NOT exist!" << endl;
		return;

	}


	// Special case: First and last Nodes are the same
	if (first == last) {

		cout << "[ " << first->getName() << " ]" << endl;
		return;

	}


	// Special case: Either Nodes are isolated Nodes
	if (isNodeIsolated(first) || isNodeIsolated(last)) {

		cout << "ERROR: Invalid path!" << endl;
		return;

	}



	// Initialize the eyeball
	Node * eyeball = first;
	eyeball->setTotalWeight(0);

	vector<Edge> list;        // eyeball's adjacency list
	minBinaryHeap eyeQueue;   // queue of upcoming potential eyeballs

	int newWeight;
	int tempWeight;

	Node * dstNode = 0;       // The destination Node
	


	// Loop through the graph while the desired Node is NOT reached and the current eyeball HAS adjacent Nodes
	while ((eyeball != last) || (!eyeQueue.isEmpty())) {

		// Get the current eyeball's adjacency list
		list = eyeball->getAdjList();

	

		// Loop through the current eyeball's adjacency list
		for (unsigned int i = 0; i < list.size(); i++) {


			// Set the destination Node
			dstNode = list[i].getDst();

			newWeight = eyeball->getTotalWeight() + list[i].getWeight();
			tempWeight = dstNode->getTotalWeight();

			// Replace the Node's total weight, if necessary
			if(newWeight < tempWeight) {

				dstNode->setChanger(eyeball);        // Who changed the Node
				dstNode->setTotalWeight(newWeight);  // The total weight
				//dstNode->setVisited();               // Trip the "visited" flag

			}

			// If the destination Node has NOT been visited, add it to the queue
			if(!(dstNode->hasBeenVisited())) {

				eyeQueue.insert(dstNode);
				dstNode->setVisited();
				
			}

		}


		Node * eyeCheck = eyeball;

		// Get the new eyeball and remove it from the queue
		if (eyeQueue.getMin() != 0) 
			eyeball = eyeQueue.popMin();

		if (eyeball == eyeCheck) {

			cout << "ERROR: Invalid path!" << endl;

			return;

		}

		// Eyeball is the destination Node
		if (eyeball == last)
			break;


	}

	
	// Get who changed the destination Node
	Node * tempChanger = last->getChanger();

	// If the destination was changed, then the path is valid
	if (tempChanger != 0) {

		
		// Shortest path is a vector of Node pointers
		vector<Node *> shortestPath;


		// Go back through the path until the beginning
		while (tempChanger != first) {

			// Put all the Nodes on the path into the vector
			shortestPath.push_back(tempChanger);
			tempChanger = tempChanger->getChanger();

		}


		shortestPath.push_back(first);

		unsigned int pathSize = shortestPath.size();
		unsigned int edgeWeight = 0;
		unsigned int pathWeight = 0;
		unsigned int pathNodes = 0;

		// Output the path
		cout << "\tWeighted Shortest Path for " << first->getName() << " -> " << last->getName() << ": " << endl << endl;
		
		int j = 0;
		cout << "\t";
		for (int i = (pathSize - 1); i >= 0; i--) {

			if (i > 0) 
				edgeWeight = shortestPath[i]->getAdjNodeEdgeWeight(shortestPath[i-1]);

			else
				edgeWeight = shortestPath[i]->getAdjNodeEdgeWeight(last);
	
			cout << "[ " << shortestPath[i]->getName() << " ]\t-- " << edgeWeight << " -->\t";
			j++;
			if (j % GROUP_BREAK == 0)
				cout << endl << "\t";
			pathWeight += edgeWeight;
			pathNodes++;
			
		
		}

		cout << "[ " << last->getName() << " ]" << endl << endl << endl;
		pathNodes++;

		cout << "\t\t\tPath Statistics: " << endl << endl;
		cout << "\tDistance: " << pathWeight      << " " << weightUnits << endl;
		cout << "\t   Nodes: " << pathNodes       << " " << nodeType    << endl;
		cout << "\t   Edges: " << (pathNodes - 1) << " " << edgeType    << endl;


	}
	

	// Invalid path
	else
		cout << "ERROR: Invalid path!" << endl;
	

}


/*
  Name: setNodeType in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Changes the name of the node type to a specified input
  Incoming: type is the specified string to be the new name of nodeType
  Outgoing: N/A
  Return: N/A
*/
void Graph::setNodeType(string type) {

	nodeType = type;

}


/*
  Name: setEdgeType in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Changes the name of the edge type to a specified input
  Incoming: type is the specified string to be the new name of edgeType
  Outgoing: N/A
  Return: N/A
*/
void Graph::setEdgeType(string type) {

	edgeType = type;

}


/*
  Name: setWeightUnits in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Changes the name of the weight units to a specified input
  Incoming: units is the specified string to be the new name of weightUnits
  Outgoing: N/A
  Return: N/A
*/
void Graph::setWeightUnits(string units) {

	weightUnits = units;

}


/*
  Name: getNodeType in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the node type.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the node type as a string.
*/
string Graph::getNodeType() const {

	return nodeType;

}


/*
  Name: getEdgeType in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the edge type.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the edge type as a string.
*/
string Graph::getEdgeType() const {

	return edgeType;

}


/*
  Name: getWeightUnits in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the weight units.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the weight units as a string.
*/
string Graph::getWeightUnits() const {

	return weightUnits;

}


/*
  Name: getArr in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the array of Node pointers made from getFromFile
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the array of Node pointers made from getFromFile
*/
Node * Graph::getArr() const {

	return arr;

}


/*
  Name: getNodeList in scope of Graph
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the list of nodes.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the list of nodes.
*/
const vector<Node *> & Graph::getNodeList() const {

	return nodeList;

}


/*
  Name: resetGraph in scope of Graph
  Writer(s): Michael Beaver
  Purpose: Resets the graph's data members to default
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void Graph::resetGraph() {

	int size = nodeList.size();

	// Reset all Nodes' "changers," total weights, and visit status
	for (int i = 0; i < size; i++) {

		nodeList[i]->setChanger(0);
		nodeList[i]->setTotalWeight(INT_MAX);   // INT_MAX = logical infinity
		
		if (nodeList[i]->hasBeenVisited())
			nodeList[i]->setVisited();

	}

}


/*
  Name: clearGraph in scope of Graph
  Writer(s): Michael Beaver
  Purpose: Clears all nodes from the graph and sets everything back to default
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void Graph::clearGraph() {

	delete [] arr;
	arr = 0;

	nodeList.clear();

	edgeType = "edges";
	nodeType = "vertices";
	weightUnits = "units";

}
