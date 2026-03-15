# LinkedIn Job Alert Bot

A .NET background service that monitors LinkedIn job postings and sends Telegram notifications based on custom filters.

## Features

- LinkedIn job scraping
- Company filtering
- Location filtering
- Tech stack filtering
- Last 3 days job detection
- Duplicate job prevention
- Telegram notifications

## Tech Stack

- .NET 8
- HtmlAgilityPack
- Telegram Bot API

## How It Works

1. Fetch LinkedIn jobs
2. Filter by company, location, tech stack
3. Ignore already processed jobs
4. Send alert to Telegram

## Setup

1. Clone repository
2. Add your Telegram bot token in `appsettings.json`
3. Run project
