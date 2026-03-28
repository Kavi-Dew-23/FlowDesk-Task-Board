using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesk.Application.DTO
{
    public record CreateProjectDto(string Name, string Description);

    public record ProjectResponseDto(
        Guid Id,
        string Name,
        string Description,
        DateTime CreatedAt
    );
}
