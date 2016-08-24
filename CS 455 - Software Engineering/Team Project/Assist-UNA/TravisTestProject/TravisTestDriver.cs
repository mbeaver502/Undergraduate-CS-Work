using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TravisTestProject
{
    class TravisTestDriver
    {

        public static void Main()
        {
            Console.WriteLine("Which would you like to test:");
            Console.WriteLine("1. Assembler \n2. Symbol Table\n3. Literal Table\n4. Machine Op Table");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                //MachineOpTableTest.Initialize();

                /* Allow for testing on other's machines. */
                Console.Write("Use default on Trav's computer? (y/n): ");
                choice = Console.ReadLine();
                while (choice != "y" && choice != "n")
                    choice = Console.ReadLine();

                string source;
                string prt;
                string intermediate;
                string obj;
                string currDirectory;

                if (choice == "y")
                {
                    //source = new FileStream("C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                    //                        "\\Software Engineering\\Team Project\\Test Files" +
                    //                        "\\Assembler\\AssemblerTest.txt",
                    //                        FileMode.Open, FileAccess.Read);
                    source = "C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                                           "\\Software Engineering\\Team Project\\Test Files" +
                                           "\\Assembler\\MBPROG7.una";

                    //prt = new FileStream("C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                    //                              "\\Software Engineering\\Team Project\\Test Files" +
                    //                              "\\Assembler\\Assembler.PRT",
                    //                              FileMode.Create, FileAccess.Write);
                    prt = "C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                                                  "\\Software Engineering\\Team Project\\Test Files" +
                                                  "\\Assembler\\AssemblerTest.PRT";

                    //intermediate = new FileStream("C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                    //                              "\\Software Engineering\\Team Project\\Test Files" +
                    //                              "\\Assembler\\TestIntermediateFile.txt",
                    //                              FileMode.Create, FileAccess.ReadWrite);
                    intermediate = "C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                                                  "\\Software Engineering\\Team Project\\Test Files" +
                                                  "\\Assembler\\AssemblerTest.imf";

                    obj = "C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                                                  "\\Software Engineering\\Team Project\\Test Files" +
                                                  "\\Assembler\\AssemblerTest.obj";

                    currDirectory = "C:\\Users\\Travis\\Documents\\Courses\\Spring 2014" +
                                                  "\\Software Engineering\\Team Project\\Test Files" +
                                                  "\\Assembler\\";
                }

                else
                {
                    Console.WriteLine("Enter path of source code, NO EXTENSION " + 
                                      "(only one chance!): ");
                    string fileName = Console.ReadLine();
                    //source = new FileStream(fileName + ".txt", FileMode.Open, FileAccess.Read);
                    source = fileName + ".una";
                    prt = fileName + ".PRT";
                    //prt = new FileStream(fileName + ".PRT", FileMode.Create, FileAccess.Write);
                    //intermediate = new FileStream(Console.ReadLine(), FileMode.Create, 
                    //                              FileAccess.ReadWrite);
                    //Console.WriteLine("Enter path for intermediate file, NO EXTENSION " +
                    //                  "(only one chance!): ");
                   intermediate = fileName + ".imf";
                   //Console.WriteLine("Enter path for object file, NO EXTENSION " +
                   //                  "(only one chance!): ");
                   obj = fileName + ".obj";
                }

                //LiteralTable testLiteralTable = new LiteralTable();
                //SymbolTable testSymbolTable = new SymbolTable();

                string identifier = "TRAVIS HUNT";

                //AssemblerTest assembler = new AssemblerTest(identifier,source, prt, intermediate, obj,
                //                                            testSymbolTable, testLiteralTable, 9000,
                //                                            500, 900);

                AssemblerTest assembler = new AssemblerTest(identifier, source, prt, intermediate, obj,
                                                            9000, 500, 900);

                Console.WriteLine("Start pass 1...");

                /* Actual testing of translator pass 1. */
                assembler.Pass1();

                Console.WriteLine("Reading file complete.");

                assembler.PrintErrorStream();

                Console.WriteLine("Pass 1 complete.");
                Console.WriteLine("Start pass 2...");

                assembler.Pass2();

                Console.WriteLine("Pass 2 complete.");
                

                Console.WriteLine();
                //testLiteralTable.PrintTable();
                //testSymbolTable.PrintTable();

                //File.Delete(intermediate);
                //File.Delete(obj);
            }

            else if(choice == "2")
            {
                SymbolTable symbolTable = new SymbolTable();
                symbolTable.Hash("START", "000000");
                symbolTable.Hash("PRNTLABE", "0000A3");
                Console.WriteLine();
                symbolTable.PrintTable();
            }

            else if (choice == "3")
            {
                LiteralTable literalTable = new LiteralTable();
                literalTable.Hash("=C\'0THIS IS A LINE\'", "00000C");
                literalTable.Hash("=C\'1SO IS THIS ONE\'", "0000A0");
                Console.WriteLine();
                literalTable.PrintTable();
            }

            else
            {
                //MachineOpTableTest.Initialize();
                string op = "";
                Console.Write("\nEnter operation to look up (0 to exit): ");
                op = Console.ReadLine().ToUpper();
                while (op != "0")
                {
                    int index = MachineOpTableTest.IsOpcode(op);

                    if (index >= 0)
                    {
                        Console.WriteLine("Code for {0}: {1}", op,
                                          MachineOpTableTest.GetObjectCode(index));
                    }

                    else
                        Console.WriteLine("The code you entered does not exist.");

                    Console.Write("\nEnter operation to look up (0 to exit): ");
                    op = Console.ReadLine().ToUpper();
                }
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
