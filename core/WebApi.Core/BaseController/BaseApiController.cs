﻿using System.Linq.Dynamic.Core;

using Application.Core.ApplicationServices;
using Application.Core.DTO;

using Domain.Core.Models;

using Microsoft.AspNetCore.Mvc;

namespace WebApi.Core.BaseController;

/// <summary>
/// 通用控制器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BaseApiController<TEntity,TKey> : ControllerBase 
    where TEntity : AggregateRoot<TKey>
    where TKey : struct
{
    /// <summary>
    /// 通用泛型服务
    /// </summary>
    protected IBaseService<TEntity, TKey> Service { get; }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="service">应用层服务</param>
    public BaseApiController(IBaseService<TEntity, TKey> service)
    {
        Service = service;
    }

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <remarks>支持动态排序，动态筛选</remarks>
    /// <param name="searchParams">通用查询参数</param>
    /// <returns></returns>
    [HttpGet]
    public PagedResult<dynamic> Get([FromQuery] SearchParams searchParams) => Service.GetPagedResult(searchParams);

    /// <summary>
    /// 根据主键获取
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("GetModelById")]
    public async Task<TEntity?> GetModelById(TKey id, CancellationToken cancellationToken = default) => await Service.GetModelById(id, cancellationToken);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="model">模型参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(TEntity model, CancellationToken cancellationToken = default) => Ok(await Service.AddEntity(model, cancellationToken));

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="models">模型列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("BulkAdd")]
    public async Task<IActionResult> BulkAdd(List<TEntity> models, CancellationToken cancellationToken = default)
    {
        await Service.BulkAddEntity(models, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model">模型参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put(TEntity model, CancellationToken cancellationToken = default) => Ok(await Service.EditEntity(model, cancellationToken));
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(TKey id, CancellationToken cancellationToken = default) => Ok(await Service.DeleteEntity(id, cancellationToken));
}