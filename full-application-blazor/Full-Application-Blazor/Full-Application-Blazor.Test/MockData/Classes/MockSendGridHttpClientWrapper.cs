using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Test.MockData.Classes
{
    [ExcludeFromCodeCoverage]
    public class MockSendGridHttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public Uri BaseAddress
        {
            get
            {
                return _httpClient.BaseAddress;
            }
            set
            {
                _httpClient.BaseAddress = value;
            }
        }

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                return _httpClient.DefaultRequestHeaders;
            }
        }

        public MockSendGridHttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string? requestUrl)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    @"{
                            'id': 'd - ab808e13fd6e443cafcde50a2fae493a',
                            'name': 'Creativ360 Contact Us',
                            'generation': 'dynamic',
                            'updated_at': '2022-07-17 10:40:19',
                            'versions': 
                            [{
                                'id': '075424a7-51cc-4763-9b50-81c98cf604ef',
                                'user_id': 27394322,
                                'template_id': 'd-ab808e13fd6e443cafcde50a2fae493a',
                                'active': 1,
                                'name': 'Creativ360ContactUs',
                                'html_content': '<!DOCTYPE html PUBLIC \' -//W3C//DTD XHTML 1.0 Strict//EN\' \'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\' >\n<html data-editor-version=\'2\' class=\'sg -campaigns\' xmlns =\'http://www.w3.org/1999/xhtml\' >\n    <head>\n      <meta http-equiv=\'Content -Type\' content =\'text /html; charset=utf-8\' >\n      <meta name=\'viewport\' content =\'width =device-width, initial-scale=1, minimum-scale=1, maximum-scale=1\' >\n      <!--[if !mso]><!-->\n      <meta http-equiv=\'X -UA-Compatible\' content =\'IE =Edge\' >\n      <!--<![endif]-->\n      <!--[if (gte mso 9)|(IE)]>\n      <xml>\n        <o:OfficeDocumentSettings>\n          <o:AllowPNG/>\n          <o:PixelsPerInch>96</o:PixelsPerInch>\n        </o:OfficeDocumentSettings>\n      </xml>\n      <![endif]-->\n      <!--[if (gte mso 9)|(IE)]>\n  <style type=\'text /css\' >\n    body {width: 600px;margin: 0 auto;}\n    table {border-collapse: collapse;}\n    table, td {mso-table-lspace: 0pt;mso-table-rspace: 0pt;}\n    img {-ms-interpolation-mode: bicubic;}\n  </style>\n<![endif]-->\n      <style type=\'text /css\' >\n    body, p, div {\n      font-family: arial,helvetica,sans-serif;\n      font-size: 14px;\n    }\n    body {\n      color: #000000;\n    }\n    body a {\n      color: #1188E6;\n      text-decoration: none;\n    }\n    p { margin: 0; padding: 0; }\n    table.wrapper {\n      width:100% !important;\n      table-layout: fixed;\n      -webkit-font-smoothing: antialiased;\n      -webkit-text-size-adjust: 100%;\n      -moz-text-size-adjust: 100%;\n      -ms-text-size-adjust: 100%;\n    }\n    img.max-width {\n      max-width: 100% !important;\n    }\n    .column.of-2 {\n      width: 50%;\n    }\n    .column.of-3 {\n      width: 33.333%;\n    }\n    .column.of-4 {\n      width: 25%;\n    }\n    ul ul ul ul  {\n      list-style-type: disc !important;\n    }\n    ol ol {\n      list-style-type: lower-roman !important;\n    }\n    ol ol ol {\n      list-style-type: lower-latin !important;\n    }\n    ol ol ol ol {\n      list-style-type: decimal !important;\n    }\n    @media screen and (max-width:480px) {\n      .preheader .rightColumnContent,\n      .footer .rightColumnContent {\n        text-align: left !important;\n      }\n      .preheader .rightColumnContent div,\n      .preheader .rightColumnContent span,\n      .footer .rightColumnContent div,\n      .footer .rightColumnContent span {\n        text-align: left !important;\n      }\n      .preheader .rightColumnContent,\n      .preheader .leftColumnContent {\n        font-size: 80% !important;\n        padding: 5px 0;\n      }\n      table.wrapper-mobile {\n        width: 100% !important;\n        table-layout: fixed;\n      }\n      img.max-width {\n        height: auto !important;\n        max-width: 100% !important;\n      }\n      a.bulletproof-button {\n        display: block !important;\n        width: auto !important;\n        font-size: 80%;\n        padding-left: 0 !important;\n        padding-right: 0 !important;\n      }\n      .columns {\n        width: 100% !important;\n      }\n      .column {\n        display: block !important;\n        width: 100% !important;\n        padding-left: 0 !important;\n        padding-right: 0 !important;\n        margin-left: 0 !important;\n        margin-right: 0 !important;\n      }\n      .social-icon-column {\n        display: inline-block !important;\n      }\n    }\n  </style>\n    <style>\n      @media screen and (max-width:480px) {\n        table\\0 {\n          width: 480px !important;\n          }\n      }\n    </style>\n      <!--user entered Head Start--><!--End Head user entered-->\n    </head>\n    <body>\n      <center class=\'wrapper\' data -link-color=\'#1188E6\' data -body-style=\'font -size:14px; font-family:arial,helvetica,sans-serif; color:#000000; background-color:#FFFFFF;\' >\n        <div class=\'webkit\' >\n          <table cellpadding=\'0\' cellspacing =\'0\' border =\'0\' width =\'100 %\' class=\'wrapper\' bgcolor =\'#FFFFFF\' >\n            <tr>\n              <td valign=\'top\' bgcolor =\'#FFFFFF\' width =\'100 %\' >\n                <table width=\'100 %\' role =\'content -container\' class=\'outer\' align =\'center\' cellpadding =\'0\' cellspacing =\'0\' border =\'0\' >\n                  <tr>\n                    <td width=\'100 %\' >\n                      <table width=\'100 %\' cellpadding =\'0\' cellspacing =\'0\' border =\'0\' >\n                        <tr>\n                          <td>\n                            <!--[if mso]>\n    <center>\n    <table><tr><td width=\'600\' >\n  <![endif]-->\n                                    <table width=\'100 %\' cellpadding =\'0\' cellspacing =\'0\' border =\'0\' style =\'width:100%; max-width:600px;\' align =\'center\' >\n                                      <tr>\n                                        <td role=\'modules -container\' style =\'padding:0px 0px 0px 0px; color:#000000; text-align:left;\' bgcolor =\'#FFFFFF\' width =\'100 %\' align =\'left\' ><table class=\'module preheader preheader-hide\' role =\'module\' data -type=\'preheader\' border =\'0\' cellpadding =\'0\' cellspacing =\'0\' width =\'100 %\' style =\'display: none !important; mso-hide: all; visibility: hidden; opacity: 0; color: transparent; height: 0; width: 0;\' >\n    <tr>\n      <td role=\'module -content\' >\n        <p></p>\n      </td>\n    </tr>\n  </table><table class=\'module\' role =\'module\' data -type=\'text\' border =\'0\' cellpadding =\'0\' cellspacing =\'0\' width =\'100 %\' style =\'table -layout: fixed;\' data -muid=\'696ff4cb-e13a-4745-847a-c35bc016b8b5\' >\n    <tbody>\n      <tr>\n        <td style=\'padding:18px 0px 18px 0px; line-height:22px; text-align:inherit;\' height =\'100 %\' valign =\'top\' bgcolor =\'\' role =\'module -content\' ><div><div style=\'font -family: inherit\' > Creativ360 Contact Us<br>\n<br>\nName: &nbsp;{{NAME}}<br>\nSurname: &nbsp;{{SURNAME}}<br>\nEmail Address: &nbsp;{{EMAIL}}<br>\nMobile Number: {{MOBILE_NUMBER}}<br>\n &nbsp;<br>\nMessage: &nbsp;{{MESSAGE}}</div><div></div></div></td>\n      </tr>\n    </tbody>\n  </table></td>\n                                      </tr>\n                                    </table>\n                                    <!--[if mso]>\n                                  </td>\n                                </tr>\n                              </table>\n                            </center>\n                            <![endif]-->\n                          </td>\n                        </tr>\n                      </table>\n                    </td>\n                  </tr>\n                </table>\n              </td>\n            </tr>\n          </table>\n        </div>\n      </center>\n    </body>\n  </html>',
                                'plain_content': 'Creativ360 Contact Us\n\nName: {{NAME}}\nSurname: {{SURNAME}}\nEmail Address: {{EMAIL}}\nMobile Number: {{MOBILE_NUMBER}}\n\nMessage: {{MESSAGE}}',
                                'generate_plain_content': true,
                                'subject': 'Creativ360 Contact Us',
                                'updated_at': '2022 -08-24 19:20:26',
                                'editor': 'design',
                                'thumbnail_url': '//us-east-2-production-thumbnail-bucket.s3.amazonaws.com/f753c58dc6017af905ef659b8ee8eae29c06ed67bfed908cec570ec26ad5efc2.png'
                            }]
                    }")
            };

        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }
    }
}