using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Options;

using RestSharp;
using RestSharp.Authenticators;

using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Infrastructure.Services;

public sealed class EmailProvider : IEmailProvider
{
  private readonly EmailSettings _emailSettings;

  public EmailProvider(IOptions<EmailSettings> emailSettings)
  {
    _emailSettings = emailSettings.Value;
  }

  public async Task SendAsync(
    List<User> users,
    string subject,
    string body,
    CancellationToken cancellationToken)
  {
    var options = new RestClientOptions()
    {
      BaseUrl = new Uri("https://api.mailgun.net/v3"),
      Authenticator = new HttpBasicAuthenticator("api", _emailSettings.MailGunApiKey),
    };

    var client = new RestClient(options);

    RestRequest request = new RestRequest();
    request.AddParameter("domain", _emailSettings.MailGunDomain, ParameterType.UrlSegment);
    request.Resource = "{domain}/messages";

    request.AddParameter("from", $"Mailgun Sandbox <postmaster@{_emailSettings.MailGunDomain}>");

    var recipients = string.Join(
      ",",
      users.Select(u => $"{u.FirstName} {u.LastName} <{u.Email}>"));
    request.AddParameter("to", recipients);

    request.AddParameter("subject", subject);
    request.AddParameter("text", body); // Html
    request.Method = Method.Post;

    var response = await client.PostAsync(request, cancellationToken);
  }
}