import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class WebApiRequestService {
    constructor(private http: HttpClient, private cookieService: CookieService) { }

    private makeAjaxCall(options) {
        options = options || {};

        var method = options.method ? options.method.toLowerCase() : 'get',
            url = this.getBaseUrl() + (options.url || ''),
            configs = {
                headers: this.getHeaders()
            },
            requestParams = [url, configs];

        method == 'post' && (requestParams.splice(1, 0, options.body || {}));

        return this.http[method].apply(this.http, requestParams);
    }

    private getBaseUrl() {
        var baseUrl = "";

        if (window.location.hostname == 'localhost') {
            baseUrl = 'http://localhost:8081/';
        };

        return baseUrl;
    }

    private getHeaders() {
        var headers = new HttpHeaders();

        try {
            var headerList = JSON.parse(this.cookieService.get('SU_Auth'));
            for (var headerType in headerList) {
                headers = headers.set(headerType, headerList[headerType]);
            };
        } catch (e) {
            headers = new HttpHeaders();
        };

        return headers;
    }

    public getTables() {
        return this.makeAjaxCall({
            method: 'GET',
            url: 'webapi/GetTables'
        });
    }

    public endSession() {
        return this.makeAjaxCall({
            method: 'GET',
            url: 'webapi/EndSession'
        });
    }
};
