using System;
using Application.Dtos;
using Domain.Entities;

namespace Tests
{
	public static class ArrangeObjects
	{
        public static Plan ListPlans(int days)
        {
            var list = new List<Plan>
            {
                new Plan(
                    label: "7 dias com um custo de R$30,00 por dia",
                    period: 7,
                    price: 30,
                    valueFine: 0.20M,
                    additionalDailyValue: 50
                ), new Plan(
                    label: "15 dias com um custo de R$30,00 por dia",
                    period: 15,
                    price: 30,
                    valueFine: 0.20M,
                    additionalDailyValue: 50
                ), new Plan(
                    label: "30 dias com um custo de R$30,00 por dia",
                    period: 30,
                    price: 30,
                    valueFine: 0.20M,
                    additionalDailyValue: 50
                ), new Plan(
                    label: "7 dias com um custo de R$30,00 por dia",
                    period: 45,
                    price: 30,
                    valueFine: 0.20M,
                    additionalDailyValue: 50
                )
            };

            return list.FirstOrDefault(x => x.Period == days);
        
        }

        public static BudGetRequestDto CreateRequestDto(Plan plan)
        {
            return new BudGetRequestDto
            {
                PlanId = plan.Id,
                ForecastDate = DateTime.Now.AddDays(plan.Period)
            };
        }

        public static void VerifyResult(Plan plan, BudGetRequestDto requestDto, BudGetResponseDto result, DateTime startDate)
        {

            // Assert
            Assert.Equal(plan.Label, result.PlanDescription);
            Assert.Equal(result.StartDate, DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            Assert.Equal(result.EndDate, DateTime.Now.AddDays(plan.Period).ToString("yyyy-MM-dd"));
            Assert.Equal(result.ForecastDate, requestDto.ForecastDate.ToString("yyyy-MM-dd"));
            Assert.Equal(result.EndDate, requestDto.ForecastDate.ToString("yyyy-MM-dd"));
            Assert.Equal(result.SubTotalRent, plan.Period * plan.Price);

            var differenceDays = (requestDto.ForecastDate.Date - startDate).Days;
            var daysNotEffective = plan.Period - differenceDays;
            var expectedFineValue = result.SubTotalRent + (plan.Price * daysNotEffective * plan.ValueFine);
            Assert.Equal(result.TotalRent, expectedFineValue);
            Assert.Contains(result.Description,"");
        }

    }
}

