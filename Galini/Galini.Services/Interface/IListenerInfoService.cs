﻿using Galini.Models.Enum;
using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IListenerInfoService
    {
        public Task<BaseResponse> CreateListenerInfo(RegisterUserRequest registerRequest, CreateListenerInfoRequest request);
        public Task<BaseResponse> GetAllListenerInfo(int page, int size, string? name, bool? sortByName, bool? sortByPrice, bool? sortByStar, TopicNameEnum? topicNameEnum, ListenerTypeEnum? listenerTypeEnum);
        public Task<BaseResponse> GetListenerInfoById(Guid id);
        public Task<BaseResponse> GetListenerInfoByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateListenerInfo(Guid id, UpdateListenerInfoRequest request);
        public Task<BaseResponse> RemoveListenerInfo(Guid id);
    }
}
