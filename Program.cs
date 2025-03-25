namespace WinWireAssignment01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Test cases
            var streamInput1 = GetTestCases();

            //Complexity - O(m + n) - while read [n - for zeroes count]
            var reOrderUtil = new MessageReOrderUtil(0);

            //Assuming one stream now
            var itemIndex = -1;

            //Solution1- Write heavy op - O(n) - memory effecient
            while (++itemIndex < streamInput1.Length)
            {
                reOrderUtil.AddToListWriteHeavy(streamInput1[itemIndex]);
            }

            PrintReOrderedQueue(reOrderUtil.GetListSimpleSol);

            //Solution2- Read heavy op O(M+N)
            itemIndex = -1;
            while (++itemIndex < streamInput1.Length)
            {
                reOrderUtil.AddToList(streamInput1[itemIndex]);
            }

            PrintReOrderedQueue(reOrderUtil.GetList);

            Console.ReadLine();
        }

        static int[] GetTestCases()
        {
            var streamInput1 = new int[] { 1, -0, 3, -0, 4 };
            //var streamInput1 = new int[] { 1, 10, 3, 10, 4 };
            //var streamInput1 = new int[] { 0, 0, 0, -1 };
            //var streamInput1 = new int[] { };//edge case

            return streamInput1;
        }


        static void PrintReOrderedQueue(Func<List<long>> messageReOrderUtil)
        {
            var reOrderedList = messageReOrderUtil();

            if(reOrderedList.Count == 0)
            {
                Console.WriteLine("Nothing in the Ordered list to print");
                return;
            }

            Console.WriteLine($"After reordering: {string.Join(',', reOrderedList)}");
        }
    }

    public class MessageReOrderUtil
    {
        //Note: Why List\Queue\Stack - O(m + n) - while read [n - for zeroes count]
        //LinkedList and array swap can also be an option - but can lead to O(mxn) whiile read
        List<long> _nonZeroList = new();

        long _valToIgnoreCnt = 0;
        int _valToMoveToLast = -1;
        long maxAllowedInQueue = 100000;

        //Solution 2:
        int _zeroCountStartIndex = 0;
        List<long> _outputSoln2 = new();

        public MessageReOrderUtil(int valToIgnore = 0) => _valToMoveToLast = valToIgnore;

        //This option is read heavy
        public void AddToList(int val)
        {
            //Check if can add further to the list as it is a stream
            if (_nonZeroList.Count >= maxAllowedInQueue)
                throw new Exception("Cannot accomodate any more data");


            if (val == _valToMoveToLast)
            {
                _valToIgnoreCnt++;
                return;
            }
            _nonZeroList.Add(val);
        }

        //THis option is write heavy - as most op happens here
        public void AddToListWriteHeavy(int val)
        {
            //cnt: 2

            //[1,0,2]
            //1
            //1,0
            //1,0,2 - and swap cnt and arr.len and increment cnt
            //1,2,0

            //[1,0,2,0,3]
            //1
            //1,0
            //1,0,2 - and swap cnt and arr.len and increment cnt
            //1,2,0,
            //1,2,0,0 (no swap op)
            //1,2,0,0,3
            //1,2,3,0,0 --swap cnt and and arr.len and increment cnt

            //Check if can add further to the list as it is a stream
            if (_nonZeroList.Count >= maxAllowedInQueue)
                throw new Exception("Cannot accomodate any more data");

            _outputSoln2.Add(val);

            if (val == _valToMoveToLast)
            {
                //Initiate to 0 since the val is 0 now
                if (_zeroCountStartIndex < 0)
                    _zeroCountStartIndex++;

                //No need to swap here
                return;
            }
            Swap(_outputSoln2, _zeroCountStartIndex, _outputSoln2.Count -1);
            _zeroCountStartIndex++;
        }

        static void Swap(List<long> input, int fromIndex, int toIndex)
        {
            //assingn to temp
            var temp = input[toIndex];
            input[toIndex] = input[fromIndex];
            input[fromIndex] = temp;
        }

        public List<long> GetList()
        {
            if (_valToIgnoreCnt == 0) 
                return _nonZeroList.ToList();//copy and send

            //copy the numbers in the new list and finally add the zeroes (or valuetoignore) to it
            var result = _nonZeroList.ToList();//copying O(m)

            var numberToIgnoreCount = 0;
            while (numberToIgnoreCount++ < _valToIgnoreCnt) //O(M + N) here
            {
                result.Add(0);
            }

            return result;
        }

        public List<long> GetListSimpleSol()
        {
            return _outputSoln2;
        }
    }
}


/*
 
Dear Candidate,
Please find the problem statement, please complete the task & share the Code & Design Diagram.

Problem Statement:
You are building an application that processes real-time messages from multiple systems using pub/sub
architecture. Each message contains an array of numbers.
Your task is to:
Process each message in the same order it arrives.
Rearrange the numbers in the array so that all the zeros are moved to the end, while keeping the order of
the other numbers unchanged.
For example:
Input: [1, 0, 3, 0, 4]
Output: [1, 3, 4, 0, 0]
 
 */