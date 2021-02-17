using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhonebookApi.Models.Models.Phonebook;
using PhonebookApi.Models.Models.Phonebook.Command;
using PhonebookApi.Models.Models.Phonebook.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PhonebookController : ControllerBase
  {
    private readonly PhonebookQueryService _phonebookQueryService;
    private readonly PhonebookCommandService _phonebookCommandService;

    public PhonebookController(PhonebookQueryService phonebookQueryService, PhonebookCommandService phonebookCommandService)
    {
      _phonebookQueryService = phonebookQueryService;
      _phonebookCommandService = phonebookCommandService;
    }

    #region GET

    [HttpGet]
    public async Task<List<Phonebook>> Get()
    {
      return await this._phonebookQueryService.GetPhonebookAsync().ConfigureAwait(false);
    }

    [HttpGet("GetPhonebookPaged")]
    public async Task<Phonebook> GetPhonebookPaged(int pageSize, int pageNo, string searchStr, string sortColumn, bool sortDesc)
    {
      return await this._phonebookQueryService.GetPhonebookWithPagedEntries(pageSize, pageNo, searchStr, sortColumn, sortDesc).ConfigureAwait(false);
    }

    [HttpGet("PhonebookExists")]
    public async Task<bool> PhonebookExists()
    {
      return await this._phonebookQueryService.GetPhonebookExistsAsync().ConfigureAwait(false);
    }

    [HttpGet("DownloadPhonebook")]
    public async Task<ActionResult<string>> DownloadPhonebook()
    {
      byte[] fileStream = await this._phonebookQueryService.DownloadPhonebook().ConfigureAwait(false);

      return Convert.ToBase64String(fileStream, 0, fileStream.Length);
    }

    #endregion

    #region POST

    [HttpPost]
    public async Task<bool> Post([FromBody] CreatePhonebookCommand phonebook)
    {
      return await this._phonebookCommandService.SavePhonebookAsync(phonebook).ConfigureAwait(false);
    }

    #endregion
  }
}