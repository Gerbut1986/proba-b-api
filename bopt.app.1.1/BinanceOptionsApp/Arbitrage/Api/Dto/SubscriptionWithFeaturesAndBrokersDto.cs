﻿// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Dto.SubscriptionWithFeaturesAndBrokersDto
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System.Collections.Generic;

namespace Arbitrage.Api.Dto
{
  public class SubscriptionWithFeaturesAndBrokersDto
  {
    public SubscriptionDto Subscription { get; set; }

    public ProductDto Product { get; set; }

    public List<SubscriptionFeatureExDto> Features { get; set; }

    public List<SubscriptionBrokerExDto> Brokers { get; set; }
  }
}
