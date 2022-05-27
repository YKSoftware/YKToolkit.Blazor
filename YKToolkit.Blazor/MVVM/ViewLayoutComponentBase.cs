﻿namespace ConfirmDialogFromVM;

using Microsoft.AspNetCore.Components;
using System.ComponentModel;

public class ViewLayoutComponentBase<T> : LayoutComponentBase
    where T : class
{
    /// <summary>
    /// 初期化処理のオーバーライド
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        this._hasInitialized = true;
    }

    private T? _dataContext;
    /// <summary>
    /// データコンテキストを取得または設定します。
    /// </summary>
    /// <remark>DI コンテナからインジェクションされ、何らかのオブジェクトが必ず格納されることを想定しています。</remark>
    [Parameter]
    [Inject]
    public T DataContext
    {
        get => this._dataContext ?? throw new ArgumentNullException();
        set
        {
            if (this._dataContext is INotifyPropertyChanged oldValue)
            {
                oldValue.PropertyChanged -= OnPropertyChanged;
            }
            this._dataContext = value;
            if (this._dataContext is INotifyPropertyChanged newValue)
            {
                newValue.PropertyChanged += OnPropertyChanged;
            }
        }
    }

    /// <summary>
    /// プロパティ変更イベントハンドラ
    /// </summary>
    /// <param name="sender">イベント発行元</param>
    /// <param name="e">イベント引数</param>
    protected virtual void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // 初期化済みなら表示更新をおこなう
        // 非同期アクセスされても良いように InvokeAsync() を使う
        if (_hasInitialized)
            InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// 初期化済みかどうか
    /// </summary>
    private bool _hasInitialized;
}
