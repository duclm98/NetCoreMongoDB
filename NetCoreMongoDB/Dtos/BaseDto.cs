﻿using NetCoreMongoDB.Models;

namespace NetCoreMongoDB.Dtos;

public class BaseDto
{
    public string CreatorId { get; set; }
    public string CreatorName { get; set; }
    public EnumValue CreatorRole { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}