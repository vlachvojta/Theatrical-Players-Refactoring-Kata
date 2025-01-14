﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        private int totalAmount = 0;
        private int volumeCredits = 0;
        private CultureInfo _cultureInfo;
        public StatementPrinter(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
        }

        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var result = PrintHeader(invoice);

            foreach(var perf in invoice.Performances)
            {
                result = CalculatePerformace(plays, result, perf);
            }
            result += PrintAmount();
            result += PrintCredits();
            return result;
        }

        private string PrintCredits()
        {
            return String.Format("You earned {0} credits\n", volumeCredits);
        }

        private string PrintAmount()
        {
            return String.Format(_cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
        }

        private static string PrintHeader(Invoice invoice)
        {
            return string.Format("Statement for {0}\n", invoice.Customer);
        }
        private string PrintSeats(Performance perf, Play play, int thisAmount)
        {
            return String.Format(_cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
        }
        private string CalculatePerformace(Dictionary<string, Play> plays, string result, Performance perf)
        {
            var play = plays[perf.PlayID];
            var thisAmount = 0;

            // add volume credits
            thisAmount = CalculateAmount(perf, play);

            // print line for this order
            result += PrintSeats(perf, play, thisAmount);
            totalAmount += thisAmount;
            return result;
        }



        private int CalculateAmount(Performance perf, Play play)
        {
            int thisAmount;
            volumeCredits += Math.Max(perf.Audience - 30, 0);

            switch (play.Type)
            {
                case "tragedy":
                    thisAmount = 40000;
                    if (perf.Audience > 30)
                    {
                        thisAmount += 1000 * (perf.Audience - 30);
                    }
                    break;
                case "comedy":
                    thisAmount = 30000;
                    if (perf.Audience > 20)
                    {
                        thisAmount += 10000 + 500 * (perf.Audience - 20);
                    }
                    thisAmount += 300 * perf.Audience;
                    // add extra credit for every ten comedy attendees
                    volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
                    break;
                default:
                    throw new Exception("unknown type: " + play.Type);
            }

            return thisAmount;
        }

        //public string PrintAsHtml(Invoice invoice, Dictionary<string, Play> plays)
        //{
        //
        //}
    }
}
