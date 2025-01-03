using Application.Dto;
using Application.Queries;
using MediatR;

namespace AlertService.Application.Features.Notifications.GetPaged;
public sealed record GetPagedNotificationsQuery(int PerPage, int Page) : PagedQuery(PerPage, Page), IRequest<PagedDto<UserNotificationDto>>;
