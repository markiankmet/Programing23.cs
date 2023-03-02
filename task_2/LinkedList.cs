using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    class Node
    {
        public object data;
        public Node next;

        public Node(object value = null)
        {
            this.data = value;
            this.next = null;
        }
    }
}


namespace Lab_2
{

    class LinkedList
    {
        public int count;
        public Node head;

        public LinkedList()
        {
            head = new Node();
            count = 0;
        }

        public void AddToTail(int data)
        {
            Node new_node = new Node(data);
            Node cur = head;
            while (cur.next != null)
            {
                cur = cur.next;
            }
            cur.next = new_node;
            count++;
        }

        public void PrintList()
        {
            Node cur = this.head;

            while (cur != null)
            {
                Console.Write(cur.data + " ");
                cur = cur.next;
            }
            Console.WriteLine();
        }

        public int GetLength()
        {
            return count;
        }

        public void InputList(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                this.AddToTail(CheckFunctions.input_integer("value for list"));
            }
        }

        public void GenerateList()
        {
            int n = CheckFunctions.input_numeric_number("dimension");
            int a; int b;
            while (true)
            {

                a = CheckFunctions.input_integer("a");
                b = CheckFunctions.input_integer("b");
                if (a == b)
                {
                    Console.WriteLine("a must not be equal to b!");
                }
                else if (a < b)
                {
                    break;
                }
                else if (a > b) { Console.WriteLine("b must be bigger that a!"); }
            }
            Random rnd = new Random();
            for (int i = 0; i < n; ++i)
            {
                this.AddToTail(rnd.Next(a, b));         
            }

        }

        public void insert()
        {
            int index_to_insert = CheckFunctions.check_index(this.count);                  
            int value_to_insert = CheckFunctions.input_integer("new value");
            Node cur = this.head;
            Node new_node = new Node(value_to_insert);
            for (int i = 1; i < index_to_insert; ++i)
            {
                if (cur != null)
                {
                    cur = cur.next;
                }
            }
            if (cur != null)
            {
                new_node.next = cur.next;
                cur.next = new_node;
            }
            ++count;
        }

        public void erase()
        {
            int index_to_erase = CheckFunctions.check_index(this.count-1);
            int cur_idx = 1;
            Node cur_node = this.head; 
            while (true)
            {
                Node last_node = cur_node;
                cur_node = cur_node.next;
                if (cur_idx == index_to_erase)
                {
                    last_node.next = cur_node.next;
                    --this.count;
                    return;
                }
                ++cur_idx;
            }
        }

        public void find_max(out int max_value, out int max_index)
        {
            max_value = Convert.ToInt32(this.head.next.data);
            max_index = 0;
            Node cur_node = this.head;
            while (cur_node.next != null)
            {
                cur_node = cur_node.next;
                if (max_value < Convert.ToInt32(cur_node.data))
                {
                    max_value = Convert.ToInt32(cur_node.data);
                }
            }
            cur_node = this.head;
            while (cur_node.next != null)
            {
                cur_node = cur_node.next;
                if (Convert.ToInt32(cur_node.data) == max_value)
                {
                    break;
                }
                if (max_value > Convert.ToInt32(cur_node.data))
                {
                    ++max_index;
                }
            }
        }

        public bool is_negative()
        {
            Node cur = this.head;
            while (cur.next != null) 
            {
                cur = cur.next;
                if (Convert.ToInt32(cur.data) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void made_kub(int max_index)
        {
            Node cur = this.head;
            for (int i = 0; i < max_index; ++i)
            {
                cur = cur.next;
                cur.data = Math.Pow(Convert.ToInt32(cur.data), 3);
            }
        }
    }
}

