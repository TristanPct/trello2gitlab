# trello2gitlab

*Convert Trello cards to GitLab issues.*

## Features

This tool converts all cards from a Trello board to GitLab issues, using Trello API and GitLab API.

Conversion rules:
- 1 card = 1 issue
- Card title is kept (but might be truncated to match the GitLab issue title limit of 250 characters)
- Description is kept (in Markdown, but might be truncated to match the GitLab issue description limit of 1048576 characters)
- Due date is kept
- Labels are kept (matching is done using options file)
- Assignee are kept (user matching is done using options file)
- Checklists are converted to Markdown and appended to the description
- Archived cards are converted to closed issues (configurable with the `trello.include` option)

Using the options file you can convert Trello labels or lists to GitLab labels or milestones.

### Impersonation

If the provided GitLab API token has Sudo right, the following information is kept:
- Issue author and creation date
- Comment author and creation date
- Issue closing user and date

**NOTE:** 

GitLab currently restrict non-admin to set date.

> **In order to keep the creation dates, all users affected by the import will temporarily receive admin privileges.**

See: https://gitlab.com/gitlab-org/gitlab/-/issues/15320

## Usage

### 1. Trello

Create a Trello REST API key and token, following this guide: https://developer.atlassian.com/cloud/trello/guides/rest-api/authorization/

### 2. GitLab

Create a GitLab API token, following this guide: https://docs.gitlab.com/ee/user/profile/personal_access_tokens.html

Create the token with `api` and (if possible) `sudo` scopes.

### 3. Options file

Create a JSON file with all the needed information: 

 Key                             | Description
---------------------------------|---
`trello`                         | Trello specific settings.
`trello.key`                     | Trello API Key.
`trello.token`                   | Trello API Token.
`trello.boardId`                 | Trello board ID.
`trello.include`                 | Specifies which cards to include. [default: `"all"`]<br />`"all"`<br />`"open"` Includes cards that are open in lists that have been archived.<br />`"visible"` Only includes cards in lists that are not closed.<br />`"closed"`
`gitlab`                         | GitLab specific settings.
`gitlab.url`                     | GitLab server URL [default: `"https://gitlab.com"`].
`gitlab.token`                   | GitLab private access token.
`gitlab.sudo`                    | Tells if the private token has sudo rights.
`gitlab.projectId`               | GitLab target project ID.
`associations`                   | Card to issue associations.
`associations.labels_labels`     | Maps Trello label ID (`string`) with GitLab label name (`string`).
`associations.lists_labels`      | Maps Trello list ID (`string`) with GitLab label name (`string`).
`associations.labels_milestones` | Maps Trello label ID (`string`) with GitLab milestone ID (`number`).
`associations.lists_milestones`  | Maps Trello list ID (`string`) with GitLab milestone ID (`number`).
`associations.members_users`     | Maps Trello member ID (`string`) with GitLab user ID (`number`).

#### Example

```json
{
    "trello": {
        "key": "AbC123CdE456",
        "token": "C1E42Ab3Cd56",
        "boardId": "A3b45612CCdE"
    },
    "gitlab": {
        "url": "https://gitlab.example.com",
        "token": "ACbdE4256C13",
        "sudo": true,
        "projectId": 42
    },
    "associations": {
        "labels_labels": {
            "5cd8226990d0c2ddc5ea5d6b": "Bug",
            "5d4d2af081e017_0dc9b54c1": "Feature"
        },
        "lists_labels": {
            "5ed922d83c519b100a79f59e": "In progress"
        },
        "labels_milestones": {
        },
        "lists_milestones": {
            "542bf7b0dcc197c6b33451b7": 3,
            "5b430a61e56aa6d9857db275": 12
        },
        "members_users": {
            "542bf7b0ecc197c6b35457b7": 10,
            "5c39ef08a7eb5f73096330a4": 5,
            "5b430a61e56ba6d9857cb275": 12,
            "5d42855a0eb4d82ebf324365": 7,
            "58f8b57a294543b89ee319bd": 2,
            "5b438503db2fe7404f32dfef": 8
        }
    }
}
```

### 4. Run the script

Built versions are in the `publish/` folder.

```
trello2gitlab path/to/options.json
```

