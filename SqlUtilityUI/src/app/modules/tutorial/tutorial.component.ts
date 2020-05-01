import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
    selector: 'tutorial',
    templateUrl: './tutorial.component.html',
    styleUrls: ['./tutorial.component.scss']
})

export class TutorialComponent {
    private videoList = [];

    constructor(private sanitizer: DomSanitizer) {
        var list = [{ Name: 'Test 1', VideoId: 'k4qVkWh1EAo' }];

        list.forEach(item => { this.addVideo(item.Name, item.VideoId,'test description.'); });
    };

    private addVideo(displayName, videoId, description) {
        var videoDetails = new VideoDetails();

        videoDetails.displayName = displayName || '';
        videoDetails.link = this.sanitizer.bypassSecurityTrustResourceUrl('https://www.youtube.com/embed/' + videoId || '');
        videoDetails.description = description;

        this.videoList.push(videoDetails);
    }
}

class VideoDetails {
    displayName: string = '';
    link: SafeResourceUrl;
    description: string = '';
}
