using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ParkingApi.Model;
using ParkingApi.Repository;

namespace ParkingApi.Services
{
    public class TicketService
    {
        private readonly ParkingApiDbContext _dbContext;

        public TicketService(ParkingApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public TicketResult BuyTicket(string licensePlate, int hours)
        {
            int maxTime = _dbContext.Settings.FirstOrDefault().HourSetting;
            var ticket = new TicketResult();
            ticket.Errors = new List<string>();
            
            if (string.IsNullOrEmpty(licensePlate))
            {
                ticket.Errors.Add("license plate field empty");
            }

            if (hours > maxTime || hours <= 0)
            {
                ticket.Errors.Add($"please choose hours between 0 and {maxTime}.");
            }

            if (CheckIfPlateIsParked(licensePlate))
            {
                ticket.Errors.Add("You already have a valid ticket");
            }

            if (ticket.Errors.Any())
            {
                return ticket;
            }

            var newTicket = new Ticket();
            newTicket.LicensePlate = licensePlate;
            newTicket.ExpiryTime = DateTime.Now.AddHours(hours);
            newTicket.Guid = Guid.NewGuid().ToString();
            newTicket.Used = false;
            newTicket.Valid = true;
            
            _dbContext.Tickets.Add(newTicket);
            _dbContext.SaveChanges();
            
            ticket.Ticket = newTicket;
            return ticket;
        }

        public TicketResult LeaveParking(string ticketId)
        {
            var result = new TicketResult();
            result.Errors = new List<string>();
            var ticket = _dbContext.Tickets.FirstOrDefault(a => a.Guid == ticketId);
            if (ticket is null)
            {
                result.Errors.Add("Invalid ticket");
                return result;
            }
            result.Ticket = ticket;
            if (ticket.Used)
            {
                result.Errors.Add("Ticket is already used");
            }
            if (ticket.ExpiryTime < DateTime.Now)
            {
                result.Errors.Add("Ticket is already expired");
            }
            if (result.Errors.Any())
            {
                return result;
            }
            result.Ticket.Used = true;
            result.Ticket.Valid = false;
            return result;
        }

        public OccupationResult GetOccupation()
        {
            var result = new OccupationResult();
            result.Messages = new List<string>();
            result.Warnings = new List<string>();
            result.Errors = new List<string>();
            
            double maximum = _dbContext.Settings.FirstOrDefault().Occupation;
            double current =  GetCurrentOccupation();
            var percentage = current / maximum * 100;
            
            var info = $"There are currently {current} out of {maximum} cars in the Parking lot: {percentage}%";
            result.Messages.Add(info);

            switch (percentage)
            {
                case 100:
                    result.Warnings.Add("parking is full");
                    break;
                case > 100:
                    result.Errors.Add("parking is over capacity");
                    break;
                case >= 90:
                    result.Warnings.Add("parking almost at maximum capacity");
                    break;
            }

            result.CurrentOccupation = (int)current;
            result.MaxOccupation = (int)maximum;

            return result;
        }

        private bool CheckIfPlateIsParked(string licensePlate)
        {
            var ticket = _dbContext.Tickets.FirstOrDefault(a => a.LicensePlate == licensePlate);
            if (ticket is null)
            {
                return false;
            }
            if (ticket.LicensePlate == licensePlate)
            {
                return true;
            }
            return false;
        }

        private int GetCurrentOccupation()
        {
            var validTickets = _dbContext.Tickets.Where(a => a.Valid == true).ToList();
            return validTickets.Count;
        }
    }
}