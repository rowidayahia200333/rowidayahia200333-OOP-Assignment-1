
namespace Part4_LeetCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Solution solve =new Solution();
            //int[] nums = [1, 2, 3, 4];
            //int result = solve.MaxOperations(nums, 5);
            //Console.WriteLine(result);
        }
    }

    public class Solution
    {
        public int MaxOperations(int[] nums, int k)
        {
            Array.Sort(nums);
            int left = 0, right = nums.Length-1 ,operation=0;

            while (left < right)
            {
                int sum = nums[left] + nums[right];
                if (sum == k)
                {
                    operation++;
                    left++;
                        right--;
                }
                else if (sum < k) left++;
                else right--;
            }
            return operation;
        }
    }
}