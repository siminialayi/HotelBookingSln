// File: HotelBookings.Domain/Common/BaseEntity.cs

using System;

namespace HotelBooking.Domain.Common
    {
    // A base class to standardize all entities across the application
    public abstract class BaseEntity
        {
        public Guid Id { get; set; }
        protected BaseEntity()
            {
            Id = Guid.NewGuid(); // Automatically generate a unique ID upon instantiation
            }
        }
    }