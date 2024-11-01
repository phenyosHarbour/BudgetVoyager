using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Budget_Tracker_App
{
    public partial class MainWindow : Window
    {
        private List<Transaction> _transactions;
        private decimal _balance;

        // Only include these fields if they are used in your calculations
        private readonly decimal _monthlyIncomeGoal = 10000;
        private readonly decimal _monthlyExpenseLimit = 8000;

        public MainWindow()
        {
            InitializeComponent();
            _transactions = new List<Transaction>();
            _balance = 0;
            UpdateBalance();
            UpdateBudgetStatus();
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture,
                    out decimal amount) && amount > 0)
            {
                string category = IncomeCategoryComboBox.Text;
                _transactions.Add(new Transaction(amount, category, TransactionType.Income));
                _balance += amount;
                UpdateBalance();
                UpdateTransactionHistory();
                UpdateBudgetStatus();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Please enter a valid amount in Rands.");
            }
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture,
                    out decimal amount) && amount > 0)
            {
                string category = ExpenseCategoryComboBox.Text;
                _transactions.Add(new Transaction(amount, category, TransactionType.Expense));
                _balance -= amount;
                UpdateBalance();
                UpdateTransactionHistory();
                UpdateBudgetStatus();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Please enter a valid amount in Rands.");
            }
        }

        private void UpdateBalance()
        {
            BalanceTextBlock.Text = $"Current Balance: R{_balance.ToString("N2", CultureInfo.CurrentCulture)}";
        }

        private void UpdateTransactionHistory()
        {
            TransactionHistoryListBox.Items.Clear();
            foreach (var transaction in _transactions)
            {
                TransactionHistoryListBox.Items.Add(transaction);
            }
        }

        private void ClearFields()
        {
            AmountTextBox.Text = "";
            IncomeCategoryComboBox.SelectedIndex = -1;
            ExpenseCategoryComboBox.SelectedIndex = -1;
        }

        private void UpdateBudgetStatus()
        {
            decimal totalIncome = 0;
            decimal totalExpenses = 0;

            foreach (var transaction in _transactions)
            {
                if (transaction.Type == TransactionType.Income)
                {
                    totalIncome += transaction.Amount;
                }
                else if (transaction.Type == TransactionType.Expense)
                {
                    totalExpenses += transaction.Amount;
                }
            }

            decimal netIncome = totalIncome - totalExpenses;
            string statusMessage = netIncome >= 0
                ? $"Net Surplus: R{netIncome.ToString("N2", CultureInfo.CurrentCulture)}"
                : $"Net Deficit: R{(-netIncome).ToString("N2", CultureInfo.CurrentCulture)}";

            BudgetStatusTextBlock.Text = $"Total Income: R{totalIncome.ToString("N2", CultureInfo.CurrentCulture)}\n" +
                                         $"Total Expenses: R{totalExpenses.ToString("N2", CultureInfo.CurrentCulture)}\n" +
                                         statusMessage;
        }


        public class Transaction
        {
            public decimal Amount { get; set; }
            private string Category { get; set; }
            public TransactionType Type { get; set; }

            public Transaction(decimal amount, string category, TransactionType type)
            {
                Amount = amount;
                Category = category;
                Type = type;
            }

            public override string ToString()
            {
                return $"{Type} - {Category}: R{Amount.ToString("N2", CultureInfo.CurrentCulture)}";
            }
        }

        public enum TransactionType
        {
            Income,
            Expense
        }
    }
}
