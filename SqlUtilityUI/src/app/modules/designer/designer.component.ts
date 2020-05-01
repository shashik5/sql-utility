import { Component } from '@angular/core';
import { WebApiRequestService } from '../../webApiRequestService';
import { ContextMenuComponent } from 'ngx-contextmenu';
import { Utility, DomHelper } from '../../utilities';

@Component({
    selector: 'designer',
    templateUrl: './designer.component.html',
    styleUrls: ['./designer.component.scss']
})

export class DesignerComponent {

    private menuItems = [
        {
            displayName: 'New',
            action: 'createNewProject',
            description: 'Create New Project'
        },
        {
            displayName: 'Open',
            action: 'loadExistingProject',
            description: 'Open Existing Project'
        },
        {
            displayName: 'Save',
            action: 'saveProject',
            description: 'Save Project'
        }
    ];

    private properties = {
        highlightDropArea: false
    };

    private supportedQueries = [
        {
            displayName: 'Select Query',
            value: 'select'
        },
        {
            displayName: 'Create Table',
            value: 'create'
        },
        {
            displayName: 'Alter Table',
            value: 'alter'
        },
        {
            displayName: 'Drop Table',
            value: 'drop'
        },
        {
            displayName: 'Insert Record',
            value: 'insert'
        },
        {
            displayName: 'Update Record',
            value: 'update'
        }
    ];

    private tablesData = [];
    private designerData = {
        QueryType: 'select',
        QueryName: '',
        QueryData: [
            {
                TableName: '',
                ColumnList: [],
                TableClause: []
            }
        ],
        QueryClause: []
    };
    private tablesInDesigner = [];

    private DESIGNER_PERFECT_SCROLLBAR_CONFIG = {
        suppressScrollX: false,
        suppressScrollY: true
    };

    constructor(private webApiRequest: WebApiRequestService) {
        webApiRequest.getTables().subscribe(data => {
            this.tablesData = data;
        });

        window.onbeforeunload = (e) => {
            this.webApiRequest.endSession();
        };
    }

    private menuHandlers = {
        createNewProject: function () {
            debugger;
        },
        loadExistingProject: function () {
            var fileControl = document.createElement('input');
            fileControl.setAttribute('type', 'file');
            fileControl.setAttribute('accept', '.sup');

            fileControl.onchange = (e: any) => {
                try {
                    var file = e.target.files[0];
                    if (file) {
                        var reader = new FileReader();
                        reader.onload = (res: any) => {
                            var text = res.target.result;
                            console.log(text);
                        };

                        reader.readAsText(file);
                    };
                } catch (e) {

                }
            };

            fileControl.click();
        },
        saveProject: function () {
            debugger;
        }
    };

    private onMenuClick = function (e) {
        var menuAction = e.target.dataset.action;
        menuAction && this.menuHandlers[menuAction] && this.menuHandlers[menuAction].call(this, e);
    }

    private onQueryTypeChange = function (e) {
        debugger;
    }

    ngOnDestroy() {
        this.webApiRequest.endSession();
    }

    private contextMenuActions = [
        {
            text: 'Add Table',
            click: (item) => { debugger; },
            enabled: (item) => true,
            visible: (item) => !item || item.hasOwnProperty('Columns')
        },
        {
            text: 'Edit Table',
            click: (item) => { debugger; },
            enabled: (item) => true,
            visible: (item) => item && item.hasOwnProperty('Columns')
        },
        {
            text: 'Import Table',
            click: (item) => { debugger; },
            enabled: (item) => true,
            visible: (item) => !item || item.hasOwnProperty('Columns')
        },
        {
            text: 'Remove Table',
            click: (item) => { debugger; },
            enabled: (item) => true,
            visible: (item) => item && item.hasOwnProperty('Columns')
        },
        {
            text: 'Add Column',
            click: (item) => { debugger; },
            enabled: (item) => true,
            visible: (item) => item && item.hasOwnProperty('DataType')
        },
        {
            text: 'Edit Column',
            click: (item) => { debugger; },
            enabled: (item) => true,
            visible: (item) => item && item.hasOwnProperty('DataType')
        },
        {
            text: 'Remove Column',
            click: (item) => { debugger; },
            enabled: (item) => true,
            visible: (item) => item && item.hasOwnProperty('DataType')
        }
    ];

    private addExistingColumnToDesigner(column, tableName, insertAt = -1) {
        var columnList = this.designerData.QueryData[0].ColumnList;

        if (columnList.findIndex(item => (item.ColumnName == column.Name && item.BelongsToTable == tableName)) == -1) {
            var data = {
                ColumnName: column.Name,
                Alias: '',
                ColumnOperation: '',
                BelongsToTable: tableName
            };

            if (insertAt == -1) {
                columnList.push(data);
            }
            else {
                columnList.splice(insertAt, 0, data);
            };

            !this.designerData.QueryData[0].TableName && (this.designerData.QueryData[0].TableName = tableName);

            this.addToTablesListInDesigner(tableName);
        };
    }

    private removeColumnFromDesigner(column): number {
        var columnList = this.designerData.QueryData[0].ColumnList,
            index = columnList.findIndex(item => (item.ColumnName == column.ColumnName && item.BelongsToTable == column.BelongsToTable));

        if (index > -1) {
            columnList.splice(index, 1);
        };

        if (Utility.unique(Utility.pluck(columnList, 'BelongsToTable')).indexOf(column.BelongsToTable) == -1) {
            var tableIndex = this.tablesInDesigner.indexOf(column.BelongsToTable),
                tableClauseIndex = this.designerData.QueryData[0].TableClause.findIndex(item => item.JoinTableName == column.BelongsToTable);

            tableIndex > -1 && this.tablesInDesigner.splice(tableIndex, 1);
            tableClauseIndex > -1 && this.designerData.QueryData[0].TableClause.splice(tableClauseIndex, 1);
        };

        return index;
    }

    private addToTablesListInDesigner(tableName) {
        if (this.tablesInDesigner.indexOf(tableName) == -1) {
            this.tablesInDesigner.push(tableName);
            this.designerData.QueryData[0].TableName != tableName && this.designerData.QueryData[0].TableClause.push({
                ClauseType: 'JOIN',
                ClauseValue: '',
                JoinType: '',
                JoinTableName: tableName
            });
        };
    }

    private removeTableFromDesigner(tableName) {
        var columnList = this.designerData.QueryData[0].ColumnList.slice();

        columnList.forEach(column => {
            column.BelongsToTable === tableName && this.removeColumnFromDesigner(column);
        });
    }

    private reorderColumn(column, moveToIndex) {
        if (this.removeColumnFromDesigner(column) > -1) {
            this.designerData.QueryData[0].ColumnList.splice(moveToIndex, 0, column);
        };
    }

    private getTargetIndex(element): number {
        var $element = DomHelper(element),
            $targetElement = $element.is('.Column') ? $element : $element.parent('.Column');

        if ($targetElement.elements.length) {
            return $targetElement.parent().find('.Column').indexOf($targetElement);
        };

        return -1;
    };

    private onTableOrColumnDrop(e) {
        var item = e.dragData,
            indexToInsertAt = this.getTargetIndex(e.nativeEvent.target);

        if (item.hasOwnProperty('Columns')) {
            // Drop whole table.
            item.Columns.forEach(column => this.addExistingColumnToDesigner(column, item.Name, indexToInsertAt++));
        }
        else if (item.hasOwnProperty('ColumnName')) {
            // Reorder column.
            this.reorderColumn(item, indexToInsertAt);
        }
        else if (Utility.isArray(item)) {
            // Drop column.
            var columnData = null,
                tableName = null;

            item.forEach(entity => {
                if (entity.hasOwnProperty('Columns')) {
                    tableName = entity.Name;
                }
                else if (entity.hasOwnProperty('DataType')) {
                    columnData = entity;
                };
            });

            columnData && tableName && this.addExistingColumnToDesigner(columnData, tableName, indexToInsertAt);
        };
    }

    private onDragStart() {
        this.properties.highlightDropArea = true;
    }

    private onDragEnd() {
        this.properties.highlightDropArea = false;
    }
}
